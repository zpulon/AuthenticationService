using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Helper
{
    public class EncryptHelper
    {
        private static readonly object _instanceLock = new object();
        private static EncryptHelper _instance = null;
        private static SymmetricAlgorithm mobjCryptoService;
        private static string _key;
        private static readonly object _cacheLock = new object();
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();
        private static MD5 Hanlder { get; } = new MD5CryptoServiceProvider();
        /// <summary>
        /// 最大缓存条数
        /// </summary>
        private static int _maxCacheNum = 10000;

        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public static EncryptHelper GetInstance()
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        mobjCryptoService = new RijndaelManaged();
                        _key = "Guz(%&as1213^^d(fa%(HilJ$lhj!y6&(*jkP87jH7";
                        _instance = new EncryptHelper();
                    }
                }
            }
            return _instance;
        }



        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public string Encrypto(string source)
        {
            if (_cache.ContainsKey(source))
            {
                return _cache[source];
            }

            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(source);

            MemoryStream ms = new MemoryStream();

            mobjCryptoService.Key = GetLegalKey();

            mobjCryptoService.IV = GetLegalIV();

            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();

            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

            cs.Write(bytIn, 0, bytIn.Length);

            cs.FlushFinalBlock();

            ms.Close();

            byte[] bytOut = ms.ToArray();
            string reval = Convert.ToBase64String(bytOut);
            lock (_cacheLock)
            {
                if (_cache.Count > _maxCacheNum)
                {
                    foreach (var it in _cache.Take(_maxCacheNum / 5))
                    {

                        _cache.Remove(it.Key);

                    }
                }
                _cache.Add(source, reval);
            }
            return reval; ;

        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string Decrypto(string source)
        {
            lock (_cacheLock)
            {
                if (_cache.Any(it => it.Value == source))
                {
                    return _cache.Single(it => it.Value == source).Key;
                }
            }

            byte[] bytIn = Convert.FromBase64String(source);

            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);

            mobjCryptoService.Key = GetLegalKey();

            mobjCryptoService.IV = GetLegalIV();

            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            StreamReader sr = new StreamReader(cs);

            return sr.ReadToEnd();

        }

        public string MD5(string source)
        {
            var data = Encoding.UTF8.GetBytes(source);
            var security = Hanlder.ComputeHash(data);
            var sb = new StringBuilder();
            foreach (var b in security)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public string MD516(string strPwd)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(strPwd)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2.ToUpper();
        }

        #region 私有函数
        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {

            string sTemp = _key;

            mobjCryptoService.GenerateKey();

            byte[] bytTemp = mobjCryptoService.Key;

            int KeyLength = bytTemp.Length;

            if (sTemp.Length > KeyLength)

                sTemp = sTemp.Substring(0, KeyLength);

            else if (sTemp.Length < KeyLength)

                sTemp = sTemp.PadRight(KeyLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(sTemp);

        }

        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {

            string sTemp = "asdfas&&dfg*$#+)*Y41sdgsdgs&*%$$$^&&GGslsadKdfK1";

            mobjCryptoService.GenerateIV();

            byte[] bytTemp = mobjCryptoService.IV;

            int IVLength = bytTemp.Length;

            if (sTemp.Length > IVLength)

                sTemp = sTemp.Substring(0, IVLength);

            else if (sTemp.Length < IVLength)

                sTemp = sTemp.PadRight(IVLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(sTemp);

        }
        #endregion
    }
}
