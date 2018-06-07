﻿using System.Threading.Tasks;

namespace KegID.DependencyServices
{
    public interface IShare
    {
        Task Show(string title, string message, string filePath);
    }

//    /// <summary>
//    /// Interface for ShareFile
//    /// </summary>
//    public interface IShareFile
//    {
//        /// <summary>
//        /// Simply share a local file on compatible services
//        /// </summary>
//        /// <param name="localFilePath">path to local file</param>
//        /// <param name="title">Title of popup on share (not included in message)</param>
//        /// <param name="view">For iPad you must pass the view paramater. The view parameter should be the view that triggers the share action, i.e. the share button.</param>
//        /// <returns>awaitable Task</returns>
//        void ShareLocalFile(string localFilePath, string title = "", object view = null);

//        /// <summary>
//        /// Simply share a file from a remote resource on compatible services
//        /// </summary>
//        /// <param name="fileUri">uri to external file</param>
//        /// <param name="fileName">name of the file</param>
//        /// <param name="title">Title of popup on share (not included in message)</param>
//        /// <param name="view">For iPad you must pass the view paramater. The view parameter should be the view that triggers the share action, i.e. the share button.</param>
//        /// <returns>awaitable bool</returns>
//        Task ShareRemoteFile(string fileUri, string fileName, string title = "", object view = null);
//    }


//    /// <summary>
//    /// Cross platform ShareFile implemenations
//    /// </summary>
//    public class CrossShareFile
//    {
//        static Lazy<IShareFile> Implementation = new Lazy<IShareFile>(() => CreateShareFile(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

//        /// <summary>
//        /// Current settings to use
//        /// </summary>
//        public static IShareFile Current
//        {
//            get
//            {
//                var ret = Implementation.Value;
//                if (ret == null)
//                {
//                    throw NotImplementedInReferenceAssembly();
//                }
//                return ret;
//            }
//        }

//        static IShareFile CreateShareFile()
//        {
//#if NETSTANDARD2_0
//            return null;
//#else
//                return new ShareFileImplementation();
//#endif
//        }

//        internal static Exception NotImplementedInReferenceAssembly()
//        {
//            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
//        }
//    }


}
