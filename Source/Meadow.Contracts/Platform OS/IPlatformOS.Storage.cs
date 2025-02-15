﻿using System.Collections.Generic;
using System.IO;

namespace Meadow
{
    public delegate void ExternalStorageEventHandler(IExternalStorage storage, ExternalStorageState state);

    /// <summary>
    /// State values for external storage devices
    /// </summary>
    public enum ExternalStorageState
    {
        /// <summary>
        /// The storage device has been inserted/mounted
        /// </summary>
        Inserted,
        /// <summary>
        /// The storage device has been ejected/unmounted
        /// </summary>
        Ejected
    }

    public interface IExternalStorage
    {
        public DirectoryInfo Directory { get; } // this or string???
        public void Eject();
    }

    public partial interface IPlatformOS
    {
        public abstract FileSystemInfo FileSystem { get; }

        /// <summary>
        /// Contains Meadow.OS File System information.
        /// </summary>
        public abstract class FileSystemInfo
        {
            /// <summary>
            /// Raised when a change on an external storage device is detected
            /// </summary>
            public event ExternalStorageEventHandler ExternalStorageEvent = delegate { };

            /// <summary>
            /// A list of available external storage devices
            /// </summary>
            public abstract IEnumerable<IExternalStorage> ExternalStorage { get; }

            /// <summary>
            /// The root OS folder for the Meadow subsystem
            /// </summary>
            public abstract string FileSystemRoot { get; }

            /// <summary>
            /// Gets the root directory of the app file system partition.
            /// </summary>
            public string UserFileSystemRoot
            {
                get => FileSystemRoot ?? "/meadow0/";
            }

            /// <summary>
            /// Gets the `/Data` directory. Use this directory to store files that
            /// require permanent persistence, such as SQL data files, even
            /// through OS deployments and Over-the-Air (OtA) updates.
            /// </summary>
            public string DataDirectory => Path.GetFullPath("Data", UserFileSystemRoot);

            /// <summary>
            /// Gets the `/Documents` directory. Use this directory to store files that
            /// require permanent persistence, such as application document files, even
            /// through OS deployments and Over-the-Air (OtA) updates.
            /// </summary>
            public string DocumentsDirectory => Path.GetFullPath("Documents", UserFileSystemRoot);

            /// <summary>
            /// Gets the `/Cache` directory. Use this directory to store
            /// semi-transient files. The contents of this folder will be erased
            /// during application updates.
            /// </summary>
            public string CacheDirectory => Path.GetFullPath("Cache", UserFileSystemRoot);

            /// <summary>
            /// Gets the `/Temp` directory. Use this directory to store transient
            /// files. 
            /// </summary>
            /// <remarks>
            /// The contents of this folder will be erased on device restart.
            /// </remarks>
            public string TempDirectory => Path.GetFullPath("Temp", UserFileSystemRoot);
        }
    }
}
