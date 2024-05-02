using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Snapshot {

    /// <summary>
    /// C# library wrapper of SnapshotLib x64-bit dll.
    /// </summary>
    public static class SnapshotWrapper {
        /// <summary>
        /// All the available return code of the library.
        /// </summary>
        enum SnapshotReturnCodes {
            OperationSuccessful = 0,
            OperationFailed = 1,
            CouldNotOpenFile = 2,
            ReadNotSuccessful = 3,
            DirectoryNotFound = 76,
            FileNotFound = 404,
        }

        #region DLL Invokes
        //Save path
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 setSavePath(string _path);
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getSavePath();

        //SMRI Handling
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 getSmri();
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void decreaseSmri();
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 getCurrentSmri();
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 deleteSmriData(UInt32 _smri);

        //Data caching and packing
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 cacheData(UInt32 _smri, Int32 _dataSize, byte[] _data, int _refSmrisSize, Int32[] _refSmris);
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getData(UInt32 _smri, out Int32 _arraySize);
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getRefSmris(UInt32 _parentSmri, out Int32 _size);
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 packData();

        //Load from file
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 setLoadFileName(string _saveFileName);
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getLoadFileName();
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 unpackData();

        //Memory cleanup
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 resetSmri();
        ///<summary>Dll method invoke</summary>
        [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int16 resetCache();
        #endregion

        #region Save Path
        /// <summary>
        /// Sets the save path inside the dll.
        /// </summary>
        /// <param name="_path">The absolute path to the save directory</param>
        /// <returns>True if the set was successful, false otherwise.</returns>
        public static bool SetSavePath(string _path) {
            try {
                Int16 ec = setSavePath(_path);
                if (ec == (Int16)SnapshotReturnCodes.DirectoryNotFound) {
                    throw new DirectoryNotFoundException($"Passed path {_path} does not exist.");
                }
                return ec == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                Debug.LogError($"Could not set the DLL save path:\n{exception}");
                return false;
            }
        }

        /// <summary>
        /// Returns the absolute save path from inside the dll.
        /// </summary>
        /// <returns>Returns the absolute save path from inside the dll.</returns>
        /// <exception cref="Exception">Could not get the current save path from DLL</exception>
        public static string GetSavePath() {
            try {
                IntPtr strPtr = getSavePath();
                return Marshal.PtrToStringAnsi(strPtr);
            } catch (Exception exception) {
                throw new Exception("Could not get the current save path from DLL:\n{0}", exception);
            }
        }
        #endregion

        #region SMRI Handling
        /// <summary>
        /// Increases and returns the global DLL SMRI used for data storing and reference preservation.
        /// </summary>
        /// <returns>A uint representing the SMRI in the DLL</returns>
        /// <exception cref="Exception">Could not retrieve SMRI from DLL.</exception>
        public static uint GetSmri() {
            try {
                return getSmri();
            } catch (Exception exception) {
                throw new Exception("Could not retrieve increased smri due to:\n{0}", exception);
            }
        }

        /// <summary>
        /// Decreases the global SMRI by 1. Use when <seealso cref="GetSmri()"/> fails.
        /// </summary>
        /// <exception cref="Exception">Could not decrease SMRI from DLL</exception>
        public static void DecreaseSmri() {
            try {
                decreaseSmri();
            } catch (Exception exception) {
                throw new Exception("Could not decrease smri due to:\n{0}", exception);
            }
        }

        /// <summary>
        /// Returns the current non-incremented global SMRI from the DLL.
        /// </summary>
        /// <exception cref="Exception">Could not retrieve current SMRI from DLL</exception>
        public static int GetCurrentSmri() {
            try {
                return getCurrentSmri();
            } catch (Exception exception) {
                throw new Exception("Could not retrieve current smri due to:\n{0}", exception);
            }
        }

        /// <summary>
        /// Deletes the data associated with the passed SMRI inside the dll.
        /// </summary>
        /// <param name="_smri">The SMRI to delete data from</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        /// <exception cref="Exception">Could not delete the smri data</exception>
        public static bool DeleteSmriData(uint _smri) {
            try {
                return deleteSmriData(_smri) == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                throw new Exception("Could not delete the smri data due to:\n{0}", exception);
            }
        }
        #endregion

        #region Data Caching - Packing
        /// <summary>
        /// Caches the passed data and references SMRIs inside the dll cache.
        /// Values are copies so it's safe to also delete them if you want.
        /// </summary>
        /// <param name="_smri">The SMRI to associate the data array to</param>
        /// <param name="_data">The data to copy over to the dll</param>
        /// <param name="_refSmris">The references SMRI this SMRI needs.</param>
        /// <returns>True if the caching was successful, false otherwise.</returns>
        public static bool CacheData(uint _smri, byte[] _data, int[] _refSmris) {
            try {
                return cacheData(_smri, _data.Length, _data, _refSmris.Length, _refSmris) == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                Debug.LogError($"Could not cache the passed (SMRI: {_smri}) to the DLL:\n{exception}");
                return false;
            }
        }

        /// <summary>
        /// Returns the associated byte array of the passed smri from the dll.
        /// </summary>
        /// <param name="_smri">The SMRI to retrieve data for</param>
        /// <returns>A byte array containing the deserialized data or null.</returns>
        public static byte[] GetData(uint _smri) {
            try {
                Int32 size = -1;
                IntPtr containerData = getData(_smri, out size);
                if (containerData == IntPtr.Zero) {
                    throw new Exception($"Passed SMRI: {_smri} returned a nullptr for container.");
                }

                byte[] data = new byte[size];
                Marshal.Copy(containerData, data, 0, size);

                return data;
            } catch (Exception exception) {
                Debug.LogError($"Could not get data for the passed SMRI: {_smri} from the DLL:\n{exception}");
                return null;
            }
        }

        /// <summary>
        /// Returns an int array containing the referenced SMRI of the passed SMRI from the dll.
        /// </summary>
        /// <param name="_parentSmri">The SMRI to retrieve referenced SMRIs for.</param>
        /// <returns>A byte array containing the data or null.</returns>
        public static int[] GetRefSmris(uint _parentSmri) {
            try {
                int size = -1;
                IntPtr ptr = getRefSmris(_parentSmri, out size);
                int[] refSmris = new int[size];
                Marshal.Copy(ptr, refSmris, 0, size);

                return refSmris;
            } catch (Exception exception) {
                Debug.LogError($"Could not get ref SMRIs for the passed SMRI: {_parentSmri} from the DLL:\n{exception}");
                return null;
            }
        }

        /// <summary>
        /// Starts the packing sequence of the cached data inside the dll.
        /// </summary>
        /// <returns>True if packing was successful, false otherwise.</returns>
        public static bool PackData() {
            try {
                return packData() == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                Debug.LogError($"Could not pack data in the DLL:\n{exception}");
                return false;
            }
        }
        #endregion

        #region Load from file
        /// <summary>
        /// Sets the save file name inside the dll.
        /// </summary>
        /// <param name="_loadFromFileName">The save file name to unpack from.</param>
        /// <returns>True if set was successful, false otherwise.</returns>
        public static bool SetLoadFileName(string _loadFromFileName) {
            try {
                Int16 ec = setLoadFileName(_loadFromFileName);
                if (ec == (Int16)SnapshotReturnCodes.FileNotFound) {
                    throw new DirectoryNotFoundException($"Passed filename {_loadFromFileName} does not exist inside the save path: {GetSavePath()}.");
                }
                return ec == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                Debug.LogError($"Could not set the DLL load from filename:\n{exception}");
                return false;
            }
        }

        /// <summary>
        /// Returns the cached save file name from inside the dll.
        /// </summary>
        /// <returns>The saved file name stored in the dll. Can be an empty string.</returns>
        /// <exception cref="Exception">Could not get the current load from filename from DLL</exception>
        public static string GetLoadFileName() {
            try {
                IntPtr strPtr = getLoadFileName();
                return Marshal.PtrToStringAnsi(strPtr);
            } catch (Exception exception) {
                throw new Exception("Could not get the current load from filename from DLL:\n{0}", exception);
            }
        }

        /// <summary>
        /// Starts the unpacking sequence inside the dll to deserialize the serialized data and store them in the dll cache.
        /// GlobalSMRI is set to be equal to the unpacked data size.
        /// Cached datas are overwritten.
        /// </summary>
        /// <returns>True if unpacking was successful, false otherwise.</returns>
        public static bool UnpackData() {
            try {
                return unpackData() == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                Debug.LogError($"Could not pack data in the DLL:\n{exception}");
                return false;
            }
        }
        #endregion

        #region Memory Cleanup
        /// <summary>
        /// Resets the DLL global SMRI back to its default value: -1.
        /// </summary>
        /// <returns>True if the reset was succesful, false otherwise with an error log.</returns>
        public static bool ResetSmri() {
            try {
                return resetSmri() == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                Debug.LogError($"Could not reset the DLL SMRI:\n{exception}");
                return false;
            }
        }

        /// <summary>
        /// Deallocates the dll data cache and clears it.
        /// Resets the set saved directory value to empty.
        /// Resets the set file name value to empty.
        /// </summary>
        /// <returns>True if the reset was successful, false otherwise.</returns>
        public static bool ResetCache() {
            try {
                return resetCache() == (Int16)SnapshotReturnCodes.OperationSuccessful;
            } catch (Exception exception) {
                Debug.LogError($"Could not reset the cache of the DLL:\n{exception}");
                return false;
            }
        }
        #endregion
    }
}
