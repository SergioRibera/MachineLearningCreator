using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LibraryPersonal
{
    public struct UtilNames
    {
        public const string VERSION_MLC = "1.0.0 a";
        public const string LEARNING_SAVED = "learning.ls";
        public const string DATA_PROYECT = "dataproyect";
        public const string DATA_SETTINGS = "settings";
        public const string TEMP_FILE = "tmp/temp.tmp";
        public const string DATA_LEARNING_FOLDER = "Learning Data/";
        public const string DATA_ASSETS_PROYECT_FOLDER = "Assets/";
    }
    public static class Datos
    {
        /// <summary>
        /// Extensión que devuelve un Objeto apartir de un string que contiene el JSON
        /// </summary>
        /// <typeparam name="T">Corresponde a la clase que devolverá</typeparam>
        /// <param name="s">Corresponde al string que contiene el JSON del objeto</param>
        /// <returns></returns>
        public static T Deserializar<T>(this string s, bool encrypt = true)
        {
            try
            {
                if (encrypt)
                    return JsonUtility.FromJson<T>(s.Desencrypt());
                return JsonUtility.FromJson<T>(s);
            }
            catch (Exception)
            {
                if (encrypt)
                    s = s.Desencrypt();
                object obj = JsonUtility.FromJson<object>(s);
                T data = default;
                foreach (var objProperties in obj.GetType().GetProperties())
                {
                    foreach (var dataProperties in data.GetType().GetProperties())
                    {
                        if (objProperties.Name == dataProperties.Name)
                            data.GetType().GetProperty(objProperties.Name).SetValue(objProperties.GetType(), objProperties.GetValue(objProperties.GetType()));
                    }
                }
                return data;
            }
        }
        /// <summary>
        /// Extensión que devuelve un JSON del objeto extensionado
        /// </summary>
        /// <param name="o">Corresponde al objeto a Serializar</param>
        /// <returns></returns>
        public static string Serializar(this object o, bool encrypt = true)
        {
            if (encrypt)
                return JsonUtility.ToJson(o).Encrypt();
            return JsonUtility.ToJson(o);
        }
        static T Instancia<T>(T obj)
        {
            if (typeof(T).IsValueType)
            {
                obj = default;
            }
            else if (typeof(T) == typeof(string))
            {
                obj = (T)Convert.ChangeType(string.Empty, typeof(T));
            }
            else
            {
                obj = Activator.CreateInstance<T>();
            }
            return obj;
        }

        internal static void CreateTempFile(string locationProyect)
        {
            string p = GetPath + UtilNames.TEMP_FILE;
            if (!Directory.Exists(GetPath + "tmp/")) Directory.CreateDirectory(GetPath + "tmp/");
            File.WriteAllText(p, locationProyect);
        }
        internal static string ReadFromTempFile()
        {
            string p = GetPath + UtilNames.TEMP_FILE;
            if (!File.Exists(p)) {
                new DialogWindow(TypeDialogWindow.ShowMessage, messageTextTittle: "Error", messageTextMessage: "No Se Puede Abrir El Proyecto".Compatibilizate()).Open();
                Thread.Sleep(2000);
                if (Application.isPlaying)
                    Application.Quit();
                throw new Exception("No se puede abrir el proyecto");
            }
            return File.ReadAllText(p);
        }
        internal static void DeleteTempFile()
        {
            string p = GetPath + UtilNames.TEMP_FILE;
            if (File.Exists(p)) File.Delete(p);
        }

        public static void CreateProyect(string name)
        {
            string path = GetPathProyects + name;
            string pathLearning = path + "/" + UtilNames.DATA_LEARNING_FOLDER;
            string pathAssets = path + "/" + UtilNames.DATA_ASSETS_PROYECT_FOLDER;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!Directory.Exists(pathLearning)) Directory.CreateDirectory(pathLearning);
            if (!Directory.Exists(pathAssets)) Directory.CreateDirectory(pathAssets);
            ProyectoMLC newProyect = new ProyectoMLC(name, path + "/");
            SaveProyect(newProyect);
        }
        public static void DeleteProyect(string p)
        {
            string[] files = Directory.GetFiles(p);
            string[] dirs = Directory.GetDirectories(p);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteProyect(dir);
            }

            Directory.Delete(p, false);
        }

        public static List<string> GetProyects()
        {
            string pathProyectos = GetPathProyects;
            if (!Directory.Exists(pathProyectos)) Directory.CreateDirectory(pathProyectos);
            return Directory.GetDirectories(pathProyectos).ToList();
        }
        public static ProyectoMLC GetProyect(string p)
        {
            string path = p + "/" + UtilNames.DATA_PROYECT + "." + ExtensionFile.mlcpd;
            if (!File.Exists(path)) return default;
            ProyectoMLC o = File.ReadAllText(path).Deserializar<ProyectoMLC>();
            return o;
        }
        public static void SaveProyect(ProyectoMLC proyecto)
        {
            string path = GetPathProyects + proyecto.proyectName + "/";
            string pathLearning = path + UtilNames.DATA_LEARNING_FOLDER;
            File.WriteAllText(path + UtilNames.DATA_PROYECT + "." + ExtensionFile.mlcpd, proyecto.Serializar());
        }

        public static string GetPath => "C:/Sergio Ribera/Machine Learning Creator/";
        public static string GetPathProyects => GetPath + "Proyects/";
        public static bool Exist(string nameFile, ExtensionFile extension)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += nameFile + "." + extension.ToString();
            return File.Exists(path);
        }
        public static T Save<T>(this object o, string nameFile, ExtensionFile extension)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += nameFile + "." + extension.ToString();
            File.WriteAllText(path, o.Serializar());
            return (T)o;
        }
        public static T Load<T>(this object o, string nameFile, ExtensionFile extension)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (o == null) return default;
            path += nameFile + "." + extension.ToString();
            if (!File.Exists(path)) return default;
            o = File.ReadAllText(path).Deserializar<T>();
            return (T)o;
        }
        public static T Load<T>(string nameFile, ExtensionFile extension)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += nameFile + "." + extension.ToString();
            if (!File.Exists(path)) throw new Exception("El archivo no existe");
            return File.ReadAllText(path).Deserializar<T>();
        }

        public static void Foreach(this List<string> l, Action<string> a)
        {
            if (l.Count > 0)
            {
                foreach (var i in l)
                {
                    a(i);
                }
            }
            else
                a(null);
        }

        static readonly string key = "Key Where Machine Learning Proyect";
        /// <summary>
        /// es =Devuelve un string encriptado
        /// </summary>
        /// <param name="text">Establece el string a Encriptar</param>
        /// <returns></returns>
        public static string Encrypt(this string text)
        {
            //arreglo de bytes donde guardaremos la llave
            byte[] keyArray;
            //arreglo de bytes donde guardaremos el texto
            //que vamos a encriptar
            byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(text);
            //se utilizan las clases de encriptación
            //provistas por el Framework
            //Algoritmo MD5
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            //se guarda la llave para que se le realice
            //hashing
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            //Algoritmo 3DAS
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            //se empieza con la transformación de la cadena
            ICryptoTransform cTransform = tdes.CreateEncryptor();

            //arreglo de bytes dond
            byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

            tdes.Clear();

            //se regresa el resultado en forma de una cadena
            return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
        }
        /// <summary>
        /// es = Devuelve un string Desencriptado
        /// </summary>
        /// <param name="text">Establece el string a Desencriptar</param>
        /// <returns></returns>
        public static string Desencrypt(this string text)
        {
            byte[] keyArray;
            //convierte el texto en una secuencia de bytes
            byte[] Array_a_Descifrar = Convert.FromBase64String(text);

            //se llama a las clases que tienen los algoritmos
            //de encriptación se le aplica hashing
            //algoritmo MD5
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

            tdes.Clear();

            //se regresa en forma de cadena
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static bool IsEmpty(string[] ss)
        {
            bool b = false;
            foreach (var s in ss)
            {
                b = string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s);
                if (b) throw new Exception(string.Format("El string  {0}  está vacio, es nulo o es solo un espacio e blanco", ss));
            }
            return b;
        }
        public static bool IsEmpty(List<string> ss)
        {
            bool b = false;
            foreach (var s in ss)
            {
                b = string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s);
                if (b) throw new Exception(string.Format("El string  {0}  está vacio, es nulo o es solo un espacio e blanco", ss));
            }
            return b;
        }
        public static bool IsEmpty(string ss)
        {
            bool b = string.IsNullOrEmpty(ss) && string.IsNullOrWhiteSpace(ss);
            if (b) throw new Exception(string.Format("El string  {0}  está vacio, es nulo o es solo un espacio e blanco", ss));
            return b;
        }

        public static bool IsSimilarString(this string s1, string s2)
        {
            bool a = false;

            if (s1.Contains(s2) || s1.ToLower().Contains(s2.ToLower()))
                a = true;

            return a;
        }
        public static string ToFirstUpper(this string s)
        {
            //Separamos el string por espacios para obtener las palabras
            string[] palabras = s.Trim().Split(' ');
            string sR = "";
            //hacemos un recorrido por el array de palabras
            for (int i = 0; i < palabras.Length; i++)
            {
                //Convertimos la palabra en array de caracteres
                char[] letters = palabras[i].ToCharArray();
                if (letters.Length > 1)
                {
                    //si es una palabra mayor a 1 letra y verificamos que son letras
                    if (char.IsLetter(letters[0]))
                        letters[0] = char.Parse(letters[0].ToString().ToUpper());//hacemos el primer caracter de la palabra mayuscula
                }
                //remplazamos la palabra por los nuevos caracteres editados
                palabras[i] = string.Join("", letters);
            }
            //recorremos nuevamente las palabras armando el texto original nuevamente
            foreach (var word in palabras)
            {
                sR += word + " ";
            }
            //recortamos espacios en blanco del principio y el final
            return sR.Trim();
        }

        public static string Format(this string format, params object[] args)
        {
            string newString = format;
            for (int i = 0; i < args.Length; i++)
            {
                string rep = "{" + i + "}";
                newString = newString.Replace(rep, args[i].ToString());
            }
            return newString;
        }
        public static string Compatibilizate(this string s)
        {
            return s.Replace(" ", "");
        }
        public static string Decompatibilizar(this string s)
        {
            //Separamos el string por espacios para obtener las palabras
            string sR = "";
            List<string> palabras = new List<string>();
            //Convertimos la palabra en array de caracteres
            char[] letters = s.ToCharArray();
            //si es una palabra mayor a 1 letra y verificamos que son letras
            for (int i = 0; i < letters.Length; i++)
            {
                if (char.IsLetter(letters[i]))
                    if (char.IsUpper(letters[i]))
                        sR += " ";
                sR += letters[i].ToString();
            }
            //recortamos espacios en blanco del principio y el final
            return sR.Trim();
        }
    }
}