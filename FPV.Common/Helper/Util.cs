using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using FPV.Common.Helper.Diagnostics;
using FPV.Common.Helper.Interfaces;

namespace FPV.Common.Helper
{
    public class Util : IUtility
    {
        public static string SerializeJson(object text)
        {
            try
            {
                if (text == null)
                {
                    return string.Empty;
                }
                return JsonConvert.SerializeObject(text);
            }
            catch (Exception e)
            {
                ExceptionLogging.LogException(e);
                return string.Empty;
            }


        }

        public static T DeserializeJson<T>(string text)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return default(T);
            }
        }
        public static object DeserializeJson2(string text)
        {
            try
            {
                return JsonConvert.DeserializeObject<object>(text);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return null;
            }


        }

        public static string ConvertToJson(dynamic data)
        {
            string json = JsonConvert.SerializeObject(data);
            json = json.Replace(@"\n", "");
            json = json.Replace(@"\r", "");
            json = json.Replace(@"\n\n", "");
            json = json.Replace(@"\""", "");
            return json;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string SecureStringToString(SecureString str)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(str);
            byte b = 1;
            int i = 0;
            string s = "";
            while (((char)b) != '\0')
            {
                b = Marshal.ReadByte(bstr, i);
                i += 2;
                s += (char)b;
            }
            Marshal.FreeBSTR(bstr);
            return s.Replace("\0", string.Empty);
        }

        public T CastObject<T, TU>(TU obj) where T : new()
        {
            T rawObject = new T();
            Type objType = obj.GetType();
            Type rawType = rawObject.GetType();
            PropertyInfo[] objPropertiesArray = objType.GetProperties();
            PropertyInfo[] rawPropertiesArray = rawType.GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                foreach (PropertyInfo rawProperty in rawPropertiesArray)
                {
                    if (objProperty.Name == rawProperty.Name)
                    {
                        if (objProperty.PropertyType != rawProperty.PropertyType)
                        {
                            if (objProperty.PropertyType.Name.Equals("String") && rawProperty.PropertyType.Name.Equals("Guid") && !string.IsNullOrEmpty(objProperty.GetValue(obj, null).ToString()))
                            {
                                rawProperty.SetValue(rawObject,
                                    // Convert.ChangeType(
                                    Guid.Parse(objProperty.GetValue(obj, null).ToString()),
                                    // objProperty.PropertyType),
                                    null);

                            }
                            else
                            {
                                //Castrar nullable By JMM
                                if (objProperty.PropertyType.Name.Contains("Nullable"))
                                {
                                    try
                                    {
                                        rawProperty.SetValue(rawObject,
                                         // Convert.ChangeType(
                                         objProperty.GetValue((object)obj ?? null),
                                         // objProperty.PropertyType),
                                         null);
                                    }
                                    catch (Exception ex)
                                    {
                                        //ExceptionLogging.LogInfo(string.Format($"Error metodo CastObject. Error: {0} "));
                                        //ExceptionLogging.LogException(ex);
                                        throw ex;
                                    }
                                }

                            }
                        }
                        else
                        {
                            rawProperty.SetValue(rawObject,
                                // Convert.ChangeType(
                                objProperty.GetValue(obj, null),
                                // objProperty.PropertyType),
                                null);

                        }
                    }
                }
            }

            return rawObject;
        }




        #region Paginador
        public static class GenericPager
        {
            public class PaginadorGenerico<T> where T : class
            {
                public int PaginaActual { get; set; }
                public int RegistrosPorPagina { get; set; }
                public int TotalRegistros { get; set; }
                public int TotalPaginas { get; set; }
                public IEnumerable<T> Resultado { get; set; }
            }
        }

        #endregion

        #region Netcommerce

        static private Byte[] m_Key = new Byte[8];
        static private Byte[] m_IV = new Byte[8];
        private const String strKey = "208ljwou##2@=?¡--sdfqpouw&$";
        static string AesKey = "RWxpZGlic0RhcmtT";

        /// <summary>
        /// Decencrita el codigo de los productos digitales de netcommerce
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecryptAES(string value)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(AesKey); //must be 16 chars                     
                var rijndael = new RijndaelManaged
                {
                    BlockSize = 128,
                    KeySize = 256,
                    Key = key,
                    IV = key,
                    Mode = CipherMode.CBC

                };

                var buffer = Convert.FromBase64String(value);
                var transform = rijndael.CreateDecryptor();
                string decrypted;
                using (var ms = new MemoryStream())
                {
                    using (
                        var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        cs.Write(buffer, 0, buffer.Length);
                        cs.FlushFinalBlock();
                        decrypted = Encoding.UTF8.GetString(ms.ToArray());
                        cs.Close();
                    }
                    ms.Close();
                }
                return decrypted;
            }
            catch { return string.Empty; }
        }

        #endregion

        #region DataAnnotation

        /// <summary>
        /// Estandariza todos los tipos de datos que puede recibir.
        /// </summary>
        public static class TipoObjetos
        {
            public const string Guid = "guid";
            public const string StrGuidNoValido = "00000000-0000-0000-0000-000000000000";
            public const string Int = "int";
            public const string String = "string";
        }

        /// <summary>
        /// Valida que la fecha no sea menor a la fecha actual
        /// </summary>
        public class DateGreaterThan : ValidationAttribute
        {
            private readonly string otherProperty;
            public DateGreaterThan(string strOtherProperty)
            {
                otherProperty = strOtherProperty;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string StartDate = otherProperty;
                DateTime dtDate = DateTime.Now;
                if (value != null)
                {
                    DateTime dtEndDate = Convert.ToDateTime(value);
                    if (dtEndDate > dtDate)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult("La fecha final debe ser mayor que la fecha actual.");
                }
                else
                {
                    return new ValidationResult("La fecha final es requerida.");
                    //return new ValidationResult("" + validationContext.DisplayName + " is required");
                }
            }
        }


        /// <summary>
        /// Valida si el value del check es true.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)] //se indica que sera usado solo para campos.
        public class ValidCheckedBox : ValidationAttribute
        {
            /// <summary>
            /// Valida el valor que llega desde el dropdownlist.
            /// </summary>
            /// <param name="value"> valor que llega de la interfaz</param>
            /// <param name="validationContext"></param>
            /// <returns></returns>
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var returnValue = new ValidationResult(ErrorMessage);
                if (value != null && value is bool && (bool)value)
                    return ValidationResult.Success;

                return returnValue;
            }




        }

        /// <summary>
        /// Valida si el value del dropdown es apropiado.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)] //se indica que sera usado solo para campos.
        public class ValidValueInDropDownList : ValidationAttribute
        {
            /// <summary>
            /// Valor no valido del dropdownlist, normalmente es el valor con el que se inicia una lista text="seleccione...",value="-1"
            /// </summary>
            public string StrValueNotValid { get; set; }

            /// <summary>
            /// Define el tipo de dato con el cual debe validarse, si el campo es un guid o un entero se coloca llamando la clase TipoObjetos
            /// </summary>
            public string StrTipoObjeto { get; set; }

            /// <summary>
            /// Valida el valor que llega desde el dropdownlist.
            /// </summary>
            /// <param name="value"> valor que llega de la interfaz</param>
            /// <param name="validationContext"></param>
            /// <returns></returns>
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var returnValue = new ValidationResult(ErrorMessage);
                if (!string.IsNullOrWhiteSpace(value.ToString()))
                {
                    try
                    {
                        if (StrTipoObjeto.ToLower().Equals(TipoObjetos.Int))
                        {
                            int intNotValidValue = int.Parse(StrValueNotValid);
                            int intValue = int.Parse(value.ToString());
                            if (intValue != intNotValidValue)
                            {
                                return ValidationResult.Success;
                            }
                        }
                        else if (StrTipoObjeto.ToLower().Equals(TipoObjetos.Guid))
                        {
                            Guid guidNotValidValue = Guid.Parse(StrValueNotValid);
                            Guid guidValue = Guid.Parse(value.ToString());
                            if (guidValue != guidNotValidValue)
                            {
                                return ValidationResult.Success;
                            }
                        }
                        else if (StrTipoObjeto.ToLower().Equals(TipoObjetos.String))
                        {
                            if (!string.IsNullOrEmpty(value.ToString()) && !value.ToString().Equals(StrValueNotValid))
                                return ValidationResult.Success;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogging.LogException(ex);
                        return returnValue;
                    }
                }
                return returnValue;
            }


        }


        #endregion

    }

}
