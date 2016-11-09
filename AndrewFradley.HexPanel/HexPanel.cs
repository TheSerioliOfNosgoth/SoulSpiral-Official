using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace HexEditor
{
    public partial class HexPanel : UserControl
    {
        int noOfColumns = 16;
        const int asciiColumnWidth = 13;
        const int hexColumnWidth = 20;
        private long startAddress = 0;
        private long streamStartAddress = 0;
        private long streamEndAddress = 0;
        private byte[] oldBytes = null;
        private byte[] newBytes = null;
        private byte[] outBytes = null;
        private string lastFileOpened = "";
        private bool inStreamingMode = false;
        private bool selectionChanging = false;
        private bool beginningStreamMode = false;
        private bool streaming = false;
        //stores whether or not there are unsaved changes so the parent form can prompt before exiting
        //and to avoid unnecessary writes
        protected bool mDataAlteredByUser = false;
        public bool DataAlteredByUser
        {
            get
            {
                return mDataAlteredByUser;
            }
        }
        public int splitterDistance
        {
            get { return splitContainer1.SplitterDistance; }
            set { splitContainer1.SplitterDistance = value; }
        }
        public bool isSplitterFixed
        {
            get { return splitContainer1.IsSplitterFixed; }
            set { splitContainer1.IsSplitterFixed = value; }
        }

        public int numberOfColumns
        {
            get { return noOfColumns; }
            set
            {
                noOfColumns = value;
                UpdateGrids();
            }
        }

        public HexPanel()
        {
            InitializeComponent();
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(DataGridView_CellEndEdit);
            dataGridView2.CellEndEdit += new DataGridViewCellEventHandler(DataGridView_CellEndEdit);
            dataGridView1.Scroll += new ScrollEventHandler(DataGridView_Scroll);
            dataGridView2.Scroll += new ScrollEventHandler(DataGridView_Scroll);
            vScrollBar1.Scroll += new ScrollEventHandler(DataGridView_Scroll);
            dataGridView1.SelectionChanged += new EventHandler(DataGridView_SelectionChanged);
            dataGridView2.SelectionChanged += new EventHandler(DataGridView_SelectionChanged);
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (selectionChanging) return;
            selectionChanging = true;

            if (sender == dataGridView1)
            {
                dataGridView2.ClearSelection();
                if (dataGridView1.SelectedCells != null &&
                    dataGridView2.RowCount > 0 &&
                    dataGridView2.ColumnCount > 0)
                {
                    for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                    {
                        dataGridView2[dataGridView1.SelectedCells[i].ColumnIndex,
                            dataGridView1.SelectedCells[i].RowIndex].Selected = true;
                    }
                }
            }
            else if (sender == dataGridView2)
            {
                dataGridView1.ClearSelection();
                if (dataGridView2.SelectedCells != null &&
                    dataGridView1.RowCount > 0 &&
                    dataGridView1.ColumnCount > 0)
                {
                    for (int i = 0; i < dataGridView2.SelectedCells.Count; i++)
                    {
                        dataGridView1[dataGridView2.SelectedCells[i].ColumnIndex,
                            dataGridView2.SelectedCells[i].RowIndex].Selected = true;
                    }
                }
            }
            selectionChanging = false;
        }

        private void DataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (inStreamingMode)
            {
                if (sender != vScrollBar1)
                {
                    return;
                }
                startAddress += (e.NewValue - e.OldValue) * noOfColumns;
                streaming = true;
                Read(
                    lastFileOpened,
                    startAddress,
                    dataGridView1.DisplayedRowCount(false) * noOfColumns
                );
                streaming = false;
                return;
            }
            if (sender == dataGridView1)
            {
                dataGridView2.FirstDisplayedScrollingRowIndex =
                    dataGridView1.FirstDisplayedScrollingRowIndex;
                vScrollBar1.Value = dataGridView1.FirstDisplayedScrollingRowIndex;
            }
            else if (sender == dataGridView2)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex =
                    dataGridView2.FirstDisplayedScrollingRowIndex;
                vScrollBar1.Value = dataGridView2.FirstDisplayedScrollingRowIndex;
            }
            else
            {
                //the next two operations will fail if there is no data to be displayed
                //unless this check is done
                if ((dataGridView1.FirstDisplayedScrollingRowIndex != -1) && (dataGridView2.FirstDisplayedScrollingRowIndex != -1))
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = vScrollBar1.Value;
                    dataGridView2.FirstDisplayedScrollingRowIndex = vScrollBar1.Value;
                }
            }
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (oldBytes == null) return;
            DataGridView dataGridView = (DataGridView)sender;

            int row = dataGridView.CurrentCell.RowIndex;
            int column = dataGridView.CurrentCell.ColumnIndex;
            int index = (row << 4) + column;

            byte oldValue = oldBytes[index];
            byte newValue = 0;

            try
            {
                if (dataGridView == dataGridView1)
                {
                    newValue = byte.Parse(
                        dataGridView.CurrentCell.Value.ToString(),
                        System.Globalization.NumberStyles.HexNumber
                    );
                }
                else
                {
                    newValue = (byte)char.Parse(
                        dataGridView.CurrentCell.Value.ToString()
                    );
                }
                newBytes[index] = newValue;
            }
            catch (Exception)
            {
                newValue = newBytes[index];
            }

            dataGridView1[column, row].Value = String.Format("{0:X2}", newBytes[index]);
            dataGridView2[column, row].Value = ((char)newBytes[index]).ToString();

            if (newValue != oldValue)
            {
                mDataAlteredByUser = true;
                dataGridView1[column, row].Style.ForeColor = Color.Red;
                dataGridView2[column, row].Style.ForeColor = Color.Red;
            }
            else
            {
                dataGridView1[column, row].Style.ForeColor = Color.Black;
                dataGridView2[column, row].Style.ForeColor = Color.Black;
            }
        }

        private string GetValidChar(char c)
        {
            if (char.IsSymbol(c) ||
                char.IsLetterOrDigit(c) ||
                char.IsPunctuation(c))
            {
                return c.ToString();
            }
            else
            {
                return " ";
            }
        }

        public void LoadTestData()
        {
            oldBytes = new byte[256];
            newBytes = new byte[256];
            for (uint i = 0; i <= 255; i++)
            {
                newBytes[i] = oldBytes[i] = (byte)i;
            }
            UpdateGrids();
        }

        private void LeaveDataBlock()
        {
            if (lastFileOpened == null) return;
            DialogResult result = MessageBox.Show(
                "Leaving data block.\r\n\r\nWould you like to save your changes?",
                "Save",
                MessageBoxButtons.YesNo
            );
            if (result == DialogResult.Yes)
            {
                Write(lastFileOpened, startAddress);
            }
        }

        public void Read(ref byte[] bytes)
        {
            outBytes = bytes;
            oldBytes = (byte[])bytes.Clone();
            newBytes = (byte[])bytes.Clone();
            startAddress = 0;
            lastFileOpened = "";
            inStreamingMode = false;
            beginningStreamMode = false;
            streamEndAddress = 0;
            streamEndAddress = 0;
            UpdateGrids();
        }

        public void Read(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Read(stream, 0, (int)stream.Length);
            stream.Close();
        }

        public void Read(string fileName, bool useStreamingMode)
        {
            inStreamingMode = useStreamingMode;
            beginningStreamMode = inStreamingMode;
            Read(fileName);
            beginningStreamMode = false;
        }

        public void Read(string fileName, long position, int noOfBytes)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Read(stream, position, noOfBytes);
            stream.Close();
        }

        public void Read(string fileName, long position, int noOfBytes, bool useStreamingMode)
        {
            inStreamingMode = useStreamingMode;
            beginningStreamMode = inStreamingMode;
            Read(fileName, position, noOfBytes);
            beginningStreamMode = false;
        }

        public void Read(FileStream stream, long position, int noOfBytes)
        {
            if (inStreamingMode && beginningStreamMode)
            {
                streamStartAddress = position;
                streamEndAddress = position + noOfBytes;
                noOfBytes = dataGridView1.DisplayedRowCount(false) * noOfColumns;
            }

            long savedPosition = stream.Position;
            stream.Position = position;
            BinaryReader reader = new BinaryReader(stream);
            oldBytes = reader.ReadBytes(
                Math.Min(noOfBytes, (int)(stream.Length - position))
            );
            newBytes = (byte[])oldBytes.Clone();
            stream.Position = savedPosition;

            startAddress = position;
            lastFileOpened = stream.Name;
            outBytes = null;

            if (streaming)
            {
                RefreshData();
            }
            else
            {
                UpdateGrids();
            }
        }

        public void Read(FileStream stream, int position, int noOfBytes, bool useStreamingMode)
        {
            inStreamingMode = useStreamingMode;
            beginningStreamMode = inStreamingMode;
            Read(stream, position, noOfBytes);
            beginningStreamMode = false;
        }

        public void Write()
        {
            if ((lastFileOpened != null) && (lastFileOpened != ""))
            {
                Write(lastFileOpened);
            }
            else if (outBytes != null)
            {
                Write(out outBytes);
            }
        }

        public void Write(out byte[] bytes)
        {
            bytes = (byte[])newBytes.Clone();
            if (bytes == outBytes)
            {
                oldBytes = (byte[])newBytes.Clone();
                RefreshData();
            }
        }

        public void Write(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            Write(stream, 0);
            stream.Close();
        }

        public void Write(string fileName, long position)
        {
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            Write(stream, position);
            stream.Close();
        }

        public void Write(FileStream stream, long position)
        {
            if (!mDataAlteredByUser)
            {
                //nothing to write
                return;
            }
            long savedPosition = stream.Position;
            stream.Position = position;
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(
                newBytes,
                0,
                Math.Min(newBytes.Length, (int)(stream.Length - position))
            );
            stream.Position = savedPosition;

            if (stream.Name == lastFileOpened)
            {
                oldBytes = (byte[])newBytes.Clone();
                RefreshData();
            }
            mDataAlteredByUser = false;
        }

        private void UpdateGrids()
        {
            // Delete the existing cells
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();

            // There aren't any bytes to draw
            if (newBytes == null) return;

            // Calculate the number of rows needed
            int noOfRows = (int)newBytes.Length / noOfColumns;
            if ((newBytes.Length % noOfColumns) != 0) noOfRows++;

            // Build the data grids
            BuildDataGrid(dataGridView1, noOfRows, 2, hexColumnWidth, true);
            BuildDataGrid(dataGridView2, noOfRows, 1, asciiColumnWidth, false);

            // Fill in the data grids
            RefreshData();

            // Update the scrollbar
            if (inStreamingMode)
            {
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = (int)(streamEndAddress - streamStartAddress) / noOfColumns;
                vScrollBar1.LargeChange = dataGridView1.DisplayedRowCount(true);
                vScrollBar1.Enabled = (vScrollBar1.Maximum > dataGridView1.DisplayedRowCount(false));
            }
            else
            {
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = noOfRows;
                vScrollBar1.LargeChange = dataGridView1.DisplayedRowCount(true);
                vScrollBar1.Enabled = (noOfRows > dataGridView1.DisplayedRowCount(false));
            }
        }

        private void BuildDataGrid(DataGridView dataGridView, int noOfRows,
            int maxInputLength, int columnWidth, bool showRowAddresses)
        {
            // Create the columns
            DataGridViewTextBoxColumn[] columns = new DataGridViewTextBoxColumn[noOfColumns];
            for (int c = 0; c < noOfColumns; c++)
            {
                columns[c] = new DataGridViewTextBoxColumn();
                columns[c].HeaderText = "";
                columns[c].MaxInputLength = maxInputLength;
                columns[c].Name = "";
                columns[c].Resizable = System.Windows.Forms.DataGridViewTriState.False;
                columns[c].Width = columnWidth;
            }
            dataGridView.Columns.AddRange(columns);

            // Create the rows
            for (int r = 0; r < noOfRows; r++)
            {
                string[] row = new string[noOfColumns];
                for (int c = 0; c < noOfColumns; c++)
                {
                    row[c] = "";
                }
                dataGridView.Rows.Add(row);
            }
            return;
        }

        private void RefreshData()
        {
            int index = 0;
            int noOfRows = (int)newBytes.Length / noOfColumns;
            if ((newBytes.Length % noOfColumns) != 0) noOfRows++;
            for (int r = 0; r < dataGridView1.RowCount; r++)
            {
                dataGridView1.Rows[r].HeaderCell.Value =
                        "0x" + String.Format("{0:X8}", startAddress + (r << 4));
                if (inStreamingMode)
                {
                    dataGridView1.Rows[r].Visible = (r < noOfRows);
                    dataGridView2.Rows[r].Visible = (r < noOfRows);
                }
                for (int c = 0; c < noOfColumns; c++)
                {
                    if (index < newBytes.Length)
                    {
                        dataGridView1[c, r].Value = String.Format("{0:X2}", newBytes[index]);
                        dataGridView2[c, r].Value = GetValidChar((char)newBytes[index]);
                    }
                    else
                    {
                        dataGridView1[c, r].Value = "00";
                        dataGridView2[c, r].Value = " ";
                    }
                    if ((index < newBytes.Length) && (newBytes[index] != oldBytes[index]))
                    {
                        dataGridView1[c, r].Style.ForeColor = Color.Red;
                        dataGridView2[c, r].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        dataGridView1[c, r].Style.ForeColor = Color.Black;
                        dataGridView2[c, r].Style.ForeColor = Color.Black;
                    }
                    if (index < newBytes.Length)
                    {
                        index++;
                    }
                }
            }
            while (index % noOfColumns != 0)
            {
                dataGridView1[index % noOfColumns, noOfRows - 1].ReadOnly = true;
                dataGridView1[index % noOfColumns, noOfRows - 1].Style.BackColor = Color.LightGray;
                dataGridView2[index % noOfColumns, noOfRows - 1].ReadOnly = true;
                dataGridView2[index % noOfColumns, noOfRows - 1].Style.BackColor = Color.LightGray;
                index++;
            }
        }

        public void Revert()
        {
            mDataAlteredByUser = false;
            newBytes = (byte[])oldBytes.Clone();
            RefreshData();
        }

        public int GetMaxRowsOnScreen()
        {
            return dataGridView1.DisplayedRowCount(false);
        }

        public int GetMaxColumnsOnScreen()
        {
            return dataGridView1.DisplayedColumnCount(false);
        }
    }
}

/*dataGridView1.Font = new Font(
   new FontFamily("Lucida Console"),
   12,
   FontStyle.Regular,
   GraphicsUnit.Pixel
);*/