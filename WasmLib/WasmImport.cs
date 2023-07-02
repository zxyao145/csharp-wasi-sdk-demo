using System.Runtime.CompilerServices;

namespace WasmLib;

public class WasmImport
{
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void HelloFromEnv(); 
    
    
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GoWasmAdd(int x, int y);
}