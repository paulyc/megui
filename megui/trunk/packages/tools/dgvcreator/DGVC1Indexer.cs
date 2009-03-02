﻿// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeGUI
{
    public class DGVC1Indexer : CommandlineJobProcessor<DGVIndexJob>
    {
public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "DGVC1Indexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is DGVIndexJob) return new DGVC1Indexer(mf.Settings.Dgvc1IndexPath);
            return null;
        }

        public DGVC1Indexer(string executableName)
        {
            executable = executableName;
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("-i \"" + job.Input + "\" -o \"" + job.Output + "\" -e -h");
                if (job.DemuxMode == 2)
                    sb.Append(" -a"); // demux everything
                return sb.ToString();
            }
        }
    }
}
