

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace enki.dict.Commom.http
{
    public class HttpClient
    {
        public static string Post(string url, string xml)
        {
           
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = 400 * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;
    
                
                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
            }
            catch (Exception e)
            {
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        public static string PostJson(string url, string json)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = 5 * 1000;
                //禁用代理
             
                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "application/json";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;


                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
            }
            catch (Exception e)
            {
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }


        public static string PostForm(string url, string strData,Dictionary<string,string> dictHead)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            string result = "";//返回结果
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = 5 * 1000;
 
                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(strData);
                request.ContentLength = data.Length;
                if (dictHead != null) {
                    foreach (var key in dictHead.Keys){
                        if (key == "User-Agent")
                        {
                            request.UserAgent = dictHead[key];
                        }
                        else if (key == "Referer") {
                            request.UserAgent = dictHead[key];
                        }
                        else
                        {
                            request.Headers.Add(key, dictHead[key]);
                        }
                        
                    }
                }


                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
            }
            catch (Exception e)
            {
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// Post提交文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string PostFile(string url, NameValueCollection param, string file)
        {
            //边界 
            string boundary = DateTime.Now.Ticks.ToString("x");
            HttpWebRequest uploadRequest = (HttpWebRequest)WebRequest.Create(url);//url为上传的地址 
            uploadRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            uploadRequest.Method = "POST";
            uploadRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            uploadRequest.KeepAlive = true;
            uploadRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            uploadRequest.Headers.Add("Authorization", "Bearer " + "Token");
            // uploadRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            uploadRequest.Timeout = 1800000;
            //禁用代理

            //不会报超时
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;
            uploadRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            WebResponse reponse;
            //创建一个内存流 
            Stream memStream = new MemoryStream();
            //确定上传的文件路径 
            if (!String.IsNullOrEmpty(file))
            {
                boundary = "--" + boundary;
                //添加上传文件参数格式边界 
                string paramFormat = boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}\r\n";
                //写上参数 
                foreach (string key in param.Keys)
                {
                    string formitem = string.Format(paramFormat, key, param[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }
                //添加上传文件数据格式边界 
                string dataFormat = boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n";
                string header = string.Format(dataFormat, "file", Path.GetFileName(file));
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                memStream.Write(headerbytes, 0, headerbytes.Length);
                //获取文件内容
                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                //将文件内容写进内存流 
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }

                fileStream.Close();

                //添加文件结束边界 
                byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n\n" + boundary + "\r\nContent-Disposition: form-data; name=\"Upload\"\r\n\nSubmit Query\r\n" + boundary + "--");
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                //设置请求长度 
                uploadRequest.ContentLength = memStream.Length;
                //获取请求写入流 
                Stream requestStream = uploadRequest.GetRequestStream();
                //将内存流数据读取位置归零 
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                //将内存流中的buffer写入到请求写入流 
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();
            }
            string result = string.Empty;
            HttpWebResponse webResponse = null;
            //获取到上传请求的响应 
            //reponse = uploadRequest.GetResponse();
            uploadRequest.Timeout = 1800000;
            webResponse = (HttpWebResponse)uploadRequest.GetResponse();
            StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr, CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //禁用代理
 
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            if (cookie != null)
            {
                request.CookieContainer = cookie;
            }
            request.KeepAlive = true;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.UTF8);
            myStreamWriter.Write(postDataStr);

            myStreamWriter.Close();
            request.Timeout = 30 * 1000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (cookie != null)
            {
                response.Cookies = cookie.GetCookies(response.ResponseUri);
            }
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            retString = RepleXml(retString);
            return retString;
        }

        public static string HttpPostHJ(string Url, string postDataStr, string cookie)
        {

            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 200;
            //设置https验证方式
            if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            //禁用代理

            request.ContentType = "application/x-amf";
            //request.ContentLength = 101;
            request.Headers["Cookie"] = cookie;
            request.Host= "cichang.hujiang.com";
            request.Headers["Origin"] = "https://cichang.hujiang.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            request.Referer = "https://cichang.hujiang.com/flash/WordTest.swf?v=170818&v=2";
            request.Accept = "*/*";
           // request.Headers["Connection"] = "keep-alive";
            Stream myRequestStream = request.GetRequestStream();
            byte[] arrOutput = { 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x23, 0x41, 0x70, 0x70, 0x5F, 0x43, 0x6F, 0x64, 0x65, 0x2E, 0x52, 0x65, 0x6D, 0x6F, 0x74, 0x65, 0x2E, 0x57, 0x6F, 0x72, 0x64, 0x52, 0x65, 0x6D, 0x6F, 0x74, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x57, 0x6F, 0x72, 0x64, 0x73, 0x00, 0x02, 0x2F, 0x32, 0x00, 0x00, 0x00, 0x32, 0x0A, 0x00, 0x00, 0x00, 0x03, 0x00, 0x40, 0xC8, 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x18, 0x61, 0x55, 0x39, 0x7A, 0x55, 0x4B, 0x6F, 0x57, 0x59, 0x53, 0x32, 0x51, 0x52, 0x7C, 0x58, 0x46, 0x69, 0x59, 0x68, 0x39, 0x2F, 0x77, 0x3D, 0x3D };
            System.IO.BufferedStream buff = new BufferedStream(myRequestStream);
            buff.Write(arrOutput, 0, arrOutput.Length);
            buff.Close();
            //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.Default);
            //myStreamWriter.Write(postDataStr);

            //myStreamWriter.Close();
            request.Timeout = 30 * 1000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            retString = RepleXml(retString);
            return retString;
        }


        /// <summary>
        /// 提交byte[]
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static byte[] HttpPostBytes(string Url, byte[] postDataStr, string cookie)
        {

            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 200;
            //设置https验证方式
            if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.Host = "cichang.hujiang.com";
            SetHeaderValue(request.Headers, "Connection", "keep-alive");
            request.ContentType = "application/x-amf";
            //request.ContentLength = 101;
            request.Headers["Cookie"] = cookie;
            
            request.Headers["Origin"] = "https://cichang.hujiang.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            request.Referer = "https://cichang.hujiang.com/flash/WordTest.swf?v=170818&v=2";
            request.Accept = "*/*";
            request.Headers["Accept-Encoding"] = "gzip, deflate, br";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9";
            // request.Headers["Connection"] = "keep-alive";
            
            Stream myRequestStream = request.GetRequestStream();
            
            System.IO.BufferedStream buff = new BufferedStream(myRequestStream);
            buff.Write(postDataStr, 0, postDataStr.Length);
            buff.Close();

            request.Timeout = 30 * 1000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream myResponseStream = response.GetResponseStream();
            System.IO.BufferedStream myStreamReader = new BufferedStream(myResponseStream);
            byte[] retByte = new byte[1024*10];
            int len = myStreamReader.Read(retByte,0,retByte.Length);
            myStreamReader.Close();
            myResponseStream.Close();
            byte[] bits = new byte[len];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = retByte[i];
            }
            return bits;
        }

        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string HttpPut(string Url, string postDataStr, CookieContainer cookie)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            if (cookie != null)
            {
                request.CookieContainer = cookie;
            }
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (cookie != null)
            {
                response.Cookies = cookie.GetCookies(response.ResponseUri);
            }
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string HttpSend(string Url, string postDataStr, string method)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        /// <summary>
        /// HttpGet
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.Proxy = null;
                request.ContentType = "text/html;charset=UTF-8";
                request.Referer = "http://dict.cn/";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch(Exception ec) {
                return "";
            }
        }

        public static string HttpGetHJ(string Url)
        {
            try
            {
                if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                //禁用代理
  
                request.ContentType = "text/html;charset=UTF-8";
                request.Headers["Origin"] = "https://cichang.hujiang.com";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                request.Referer = "https://cichang.hujiang.com/flash/WordTest.swf?v=170818&v=2";
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Cookie("HJ_SID", Guid.NewGuid().ToString(),"/", ".hjenglish.com"));
                request.CookieContainer.Add(new Cookie("HJ_UID", Guid.NewGuid().ToString(),"/", ".hjenglish.com"));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ec)
            {
                return "";
            }
        }

        public static string HttpGetCookie(string Url, string cookie)
        {
            if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.Referer = "http://dict.cn/";
            request.Headers["Cookie"] = cookie;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;

        }
        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }

        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static DateTime TimestampToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        /// <summary>
        /// 替换Xml原始字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RepleXml(string source)
        {
            Regex regex = new Regex("http://tempuri.org/\">(.*?)</");
            if (!regex.IsMatch(source))
            {
                return source;
            }
            Match mat = regex.Match(source);
            return mat.Groups[1].Value;
        }
        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr">加密的字符串</param>
        /// <returns></returns>
        public static string MD5Entry(string encypStr)
        {
            string strResult;
            MD5CryptoServiceProvider pMD5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] byteInput = Encoding.GetEncoding("utf-8").GetBytes(encypStr);
            byte[] byteOut = pMD5.ComputeHash(byteInput);
            strResult = System.BitConverter.ToString(byteOut);
            strResult = strResult.Replace("-", "").ToUpper();

            byteInput = null;
            byteOut = null;

            return strResult;
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string SHA256(string strData)
        {
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(strData);
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] retVal = sha256.ComputeHash(bytValue);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSHA256HashFromString() fail,error:" + ex.Message);
            }
        }  
    }
}
