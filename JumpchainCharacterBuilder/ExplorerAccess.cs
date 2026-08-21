using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JumpchainCharacterBuilder
{
    public static class ExplorerAccess
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, uint cidl, [In, MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl, uint dwFlags);

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern void SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string name, IntPtr bindContext, [Out] out IntPtr pidl, uint sfgaoIn, [Out] out uint psfgaoOut);

        public static void OpenFolderToFile(string filePath)
        {
            var folder = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            SHParseDisplayName(folder, IntPtr.Zero, out var parsedFolder, 0, out _);

            if (parsedFolder == IntPtr.Zero)
            {
                return;
            }

            var selectedFile = Path.GetFileName(filePath);
            SHParseDisplayName(Path.Combine(folder, selectedFile), IntPtr.Zero, out var parsedFile, 0, out _);

            var targetArray = new[] { parsedFile == IntPtr.Zero ? parsedFolder : parsedFile };
            SHOpenFolderAndSelectItems(parsedFolder, (uint)targetArray.Length, targetArray, 0);

            Marshal.FreeCoTaskMem(parsedFolder);
            if (parsedFile != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(parsedFile);
            }
        }
    }
}
