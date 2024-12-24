namespace OriginFrameWork.Common.FileToMemeryHelper;

public static class FileToMemery
{

    public static void ToMemery(string filepath, out byte[] bytes)
    {
        using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
        {
            bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
        }
    }
}
