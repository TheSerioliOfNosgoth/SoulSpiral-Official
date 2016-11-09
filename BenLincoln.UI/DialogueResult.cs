// BenLincoln.UI
// Copyright 2006-2012 Ben Lincoln
// http://www.thelostworlds.net/
//

// This file is part of BenLincoln.UI.

// BenLincoln.UI is free software: you can redistribute it and/or modify
// it under the terms of version 3 of the GNU General Public License as published by
// the Free Software Foundation.

// BenLincoln.UI is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with BenLincoln.UI (in the file LICENSE.txt).  
// If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;

namespace BenLincoln.UI
{
    public class DialogueResult
    {
        //
        // Summary:
        //     There has been no result indicated by the dialogue.
        public const int NONE = 0;
        //
        // Summary:
        //     The user selected 'Yes,' or the equivalent.
        public const int YES = 1;
        //
        // Summary:
        //     The user selected 'No,' or the equivalent.
        public const int NO = 2;
        //
        // Summary:
        //     The user has indicated that they would like the program to behave as if they had selected 'Yes' to the current type of question for the duration of the current process.
        public const int ALWAYS = 3;
        //
        // Summary:
        //     The user has indicated that they would like the program to behave as if they had selected 'No' to the current type of question for the duration of the current process.
        public const int NEVER = 4;
        //
        // Summary:
        //     The user chose to proceed to the next dialogue in the current series.
        public const int NEXT_STEP = 10;
        //
        // Summary:
        //     The user chose to go back to the previous dialogue in the current series.
        public const int PREVIOUS_STEP = 11;
        //
        // Summary:
        //     The user has chosen to commit the changes specified by a multi-dialogue series.
        public const int COMMIT = 12;
        //
        // Summary:
        //     The user has chosen to retry a failed step in a process.
        public const int RETRY_CURRENT = 20;
        //
        // Summary:
        //     The user has chosen to retry a process which has failed entirely.
        public const int RETRY_ALL = 21;
        //
        // Summary:
        //     The user has chosen to ignore the failure of one step in a process.
        public const int IGNORE = 22;
        //
        // Summary:
        //     The user has chosen to abort an entire process which has reported the failure of one or more steps.
        public const int ABORT_PROCESS = 23;


    }
}
