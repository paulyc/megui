using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

namespace MeGUI.core.details
{
    public class TaggedJob
    {
        private Job job;

        public Job Job
        {
            get { return job; }
            set { job = value; }
        }


        private JobStatus status;
        private DateTime start, end; // time the job was started / ended


        #region job queue information
        private string owningWorker;

        public string OwningWorker
        {
            get { return owningWorker; }
            set { owningWorker = value; }
        }

        public TaggedJob() { }

        internal TaggedJob(Job j)
            : this()
        {
            job = j;
        }

        private List<string> requiredJobNames;


        public void AddDependency(TaggedJob other)
        {
            // we can't have each job depending on the other
            Debug.Assert(!other.RequiredJobs.Contains(this));
            RequiredJobs.Add(other);
            other.EnabledJobs.Add(this);
        }

        /// <summary>
        /// List of jobs which need to be completed before this can be processed
        /// </summary>
        public List<string> RequiredJobNames
        {
            get { return requiredJobNames; }
            set { requiredJobNames = value; }
        }

        private List<string> enabledJobNames;
        /// <summary>
        /// List of jobs which completing this job enables
        /// </summary>
        public List<string> EnabledJobNames
        {
            get { return enabledJobNames; }
            set { enabledJobNames = value; }
        }

        [XmlIgnore]
        public List<TaggedJob> EnabledJobs = new List<TaggedJob>();

        [XmlIgnore]
        public List<TaggedJob> RequiredJobs = new List<TaggedJob>();
        #endregion

        public string EncodingSpeed = "";
        private string name;
        /// <summary>
        /// the name of this job
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// status of the job
        /// </summary>
        public JobStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// time the job was started
        /// </summary>
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }
        /// <summary>
        ///  time the job was completed
        /// </summary>
        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }

        /// <summary>
        /// gets a humanly readable status tring
        /// </summary>
        public string StatusString
        {
            get
            {
                switch (status)
                {
                    case JobStatus.WAITING:
                        return "waiting";
                    case JobStatus.PROCESSING:
                        return "processing";
                    case JobStatus.POSTPONED:
                        return "postponed";
                    case JobStatus.ERROR:
                        return "error";
                    case JobStatus.ABORTED:
                        return "aborted";
                    case JobStatus.DONE:
                        return "done";
                    case JobStatus.SKIP:
                        return "skip";
                    default:
                        return "";
                }
            }
        }
        /// <summary>
        /// filename without path of the source for this job
        /// </summary>
        public string InputFileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.Job.Input);
            }
        }
        /// <summary>
        ///  filename without path of the destination of this job
        /// </summary>
        public string OutputFileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.Job.Output);
            }
        }
    }
}
