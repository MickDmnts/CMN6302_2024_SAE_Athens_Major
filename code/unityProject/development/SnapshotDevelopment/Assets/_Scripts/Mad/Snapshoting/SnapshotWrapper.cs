using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// An one-on-one struct used for data transportation to the Snapshot DLL.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct DataContainer {
    ///<summary>The data size of the passed byte array</summary>
    public Int32 _DataSize;
    ///<summary>The SMRI corresponding to the cached data</summary>
    public UInt32 _Smri;
    ///<summary>The data to be cached to the library</summary>
    public byte[] _Data;
}

public static class SnapshotWrapper {

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    enum ErrorCodes {
        OperationSuccessful = 0,
        OperationFailed = 1,
        DirectoryNotFound = 76,
        FileNotFound = 404,
    }

    #region DLL Invokes
    //Save path
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 setSavePath(string _path);
    [DllImport("SnapshotLib.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr getSavePath();

    //SMRI Handling
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern UInt32 getSmri();
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern void decreaseSmri();
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern Int32 getCurrentSmri();

    //Data caching and packing
    [DllImport("SnapshotLib.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 cacheData(DataContainer _model);
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr getData(uint _smri, out Int32 _arraySize);
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 packData();

    //Load from file
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 setLoadFileName(string _saveFileName);
    [DllImport("SnapshotLib.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr getLoadFileName();

    //Memory cleanup
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 resetSmri();
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern Int16 resetCache();
    #endregion

    #region Save Path
    /// <summary>
    /// @TODO: Summary
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public static bool SetSavePath(string _path) {
        try {
            Int16 ec = setSavePath(_path);
            if (ec == (Int16)ErrorCodes.DirectoryNotFound) {
                throw new DirectoryNotFoundException($"Passed path {_path} does not exist.");
            }
            return ec == (Int16)ErrorCodes.OperationSuccessful;
        } catch (Exception exception) {
            Debug.LogError($"Could not set the DLL save path:\n{exception}");
            return false;
        }
    }

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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
    #endregion

    #region Data Caching - Packing
    /// <summary>
    /// Caches the passed container to the DLL library.
    /// </summary>
    /// <param name="_container">The container instance to cache</param>
    /// <returns>True if caching was succesful, false otherwise.</returns>
    public static bool CacheData(DataContainer _container) {
        try {
            return cacheData(_container) == 0;
        } catch (Exception exception) {
            Debug.LogError($"Could not cache the passed container(SMRI: {_container._Smri}) to the DLL:\n{exception}");
            return false;
        }
    }

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    /// <param name="_smri"></param>
    /// <returns></returns>
    public static DataContainer? GetData(uint _smri) {
        try {
            Int32 size = -1;
            IntPtr containerData = getData(_smri, out size);

            if (containerData == IntPtr.Zero) {
                throw new Exception($"Passed SMRI: {_smri} returned a nullptr for container.");
            }

            byte[] data = new byte[size];
            Marshal.Copy(containerData, data, 0, size);

            return new DataContainer() { _Smri = _smri, _Data = data };
        } catch (Exception exception) {
            Debug.LogError($"Could not get data for the passed SMRI: {_smri} from the DLL:\n{exception}");
            return null;
        }
    }

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    /// <returns></returns>
    public static bool PackData() {
        try {
            return packData() == (Int16)ErrorCodes.OperationSuccessful;
        } catch (Exception exception) {
            Debug.LogError($"Could not pack data in the DLL:\n{exception}");
            return false;
        }
    }
    #endregion

    #region Load from file
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_loadFromFileName"></param>
    /// <returns></returns>
    public static bool SetLoadFileName(string _loadFromFileName) {
        try {
            Int16 ec = setLoadFileName(_loadFromFileName);
            if (ec == (Int16)ErrorCodes.FileNotFound) {
                throw new DirectoryNotFoundException($"Passed filename {_loadFromFileName} does not exist inside the save path: {GetSavePath()}.");
            }
            return ec == (Int16)ErrorCodes.OperationSuccessful;
        } catch (Exception exception) {
            Debug.LogError($"Could not set the DLL load from filename:\n{exception}");
            return false;
        }
    }

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetLoadFileName() {
        try {
            IntPtr strPtr = getLoadFileName();
            return Marshal.PtrToStringAnsi(strPtr);
        } catch (Exception exception) {
            throw new Exception("Could not get the currentload from filename from DLL:\n{0}", exception);
        }
    }
    #endregion

    #region Memory Cleanup
    /// <summary>
    /// Resets the DLL global SMRI back to its default value: -1.
    /// </summary>
    /// <returns>True if the reset was succesfull, false otherwise with an error log.</returns>
    public static bool ResetSmri() {
        try {
            return resetSmri() == (Int16)ErrorCodes.OperationSuccessful;
        } catch (Exception exception) {
            Debug.LogError($"Could not reset the DLL SMRI:\n{exception}");
            return false;
        }
    }

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    /// <returns></returns>
    public static bool ResetCache() {
        try {
            return resetCache() == (Int16)ErrorCodes.OperationSuccessful;
        } catch (Exception exception) {
            Debug.LogError($"Could not reset the cache of the DLL:\n{exception}");
            return false;
        }
    }
    #endregion
}
