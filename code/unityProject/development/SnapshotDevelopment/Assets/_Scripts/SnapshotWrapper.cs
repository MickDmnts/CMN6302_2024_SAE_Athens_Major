using System;
using System.Runtime.InteropServices;

public static class SnapshotWrapper {
    [DllImport("SnapshotLib.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr getNum();
    
    /// <summary>
    /// Gets an number from the the SnapshotLib dll.
    /// </summary>
    /// <returns>And Int32 (int) with the </returns>
    public static Int32 GetNum(){
        return getNum().ToInt32();
    }
}
