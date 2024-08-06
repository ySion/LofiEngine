using System.Runtime.InteropServices;

namespace LofiEngine.Gfx;

class Mi {
      [DllImport("LofiExt.dll", EntryPoint = "mimalloc")]
      public static extern IntPtr Malloc(int size);

      [DllImport("LofiExt.dll", EntryPoint = "mifree")]
      public static extern void Free(IntPtr ptr);

      [DllImport("LofiExt.dll", EntryPoint = "debug_printf")]
      public static extern void DPrint(IntPtr ptr);

      [DllImport("LofiExt.dll", EntryPoint = "debug_mprintf")]
      public static extern void MPrint(IntPtr ptr, int size);
}

unsafe class ToPointerStringList : IDisposable {
      public ToPointerStringList(IList<string> strings) {
            Count = (uint)strings.Count;
            Pointer = (byte**)Mi.Malloc(strings.Count * sizeof(byte*));

            for (int i = 0; i < strings.Count; i++) {
                  byte[] bytes = new byte[strings[i].Length + 1];
                  System.Text.Encoding.ASCII.GetBytes(strings[i], 0, strings[i].Length, bytes, 0);
                  byte* stringMem = (byte*)Mi.Malloc(bytes.Length);
                  Marshal.Copy(bytes, 0, (IntPtr)stringMem, bytes.Length);
                  Pointer[i] = stringMem;
            }

            Mi.MPrint((IntPtr)Pointer, strings.Count);
      }

      public void Dispose() {
            for (int i = 0; i < Count; i++) {
                  Mi.Free((IntPtr)Pointer[i]);
            }

            Mi.Free((IntPtr)Pointer);
            GC.SuppressFinalize(this);
      }
      
      public byte** Pointer { get; }
      public uint Count { get; }
}

unsafe class ToPointerString : IDisposable {
      public ToPointerString(string str) {
            byte[] bytes = new byte[str.Length + 1];
            System.Text.Encoding.ASCII.GetBytes(str, 0, str.Length, bytes, 0);
            pointer = (byte*)Mi.Malloc(bytes.Length * sizeof(byte));
            Marshal.Copy(bytes, 0, (IntPtr)pointer, bytes.Length);
      }

      public void Dispose() {
            Mi.Free((IntPtr)pointer);
            GC.SuppressFinalize(this);
      }

      public byte* pointer { get; }
}