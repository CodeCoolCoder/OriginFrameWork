using System.Security.Cryptography;
using System.Text;

namespace OriginFrameWork.Core.Utils;

public static class MD5Helper
{
    /// <summary>
    /// 明文密码转MD5加密，为什么用this.str,这样传的参就是前面的字符串，类似 a.tostring();括号里没有传参，参数就是前面的a
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToMD5(this string str)
    {
        MD5 mD5 = new MD5CryptoServiceProvider();
        byte[] bytes = mD5.ComputeHash(Encoding.Default.GetBytes(str + "@JG013471"));
        var md5str = BitConverter.ToString(bytes).Replace("-", "");
        return md5str;
    }
}
