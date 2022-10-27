using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecryptText
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string opcionMenu;
            do
            {
                string textoEncriptar;
                string keyEncriptacion;
                string cadenaEncriptada;
                byte[] textoEncriptado;
                Console.Clear();
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("-- Encriptación y desencriptación de cadenas ---");
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("Menú de opciones:");
                Console.WriteLine("1. Encriptar cadena");
                Console.WriteLine("2. Desencriptar cadena");
                Console.WriteLine("3. Salir");
                Console.Write("\nElija una opción: ");
                opcionMenu = Console.ReadLine();
                switch (opcionMenu)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Ingrese la cadena a encriptar: ");
                        textoEncriptar = Console.ReadLine();
                        Console.WriteLine("Ingrese la llave de encriptación, debe ser de longitud 16 o 32: ");
                        keyEncriptacion = Console.ReadLine();
                        if(keyEncriptacion.Length==16 || keyEncriptacion.Length == 32)
                        {
                            try
                            {
                                using (Aes aes = Aes.Create())
                                {
                                    if (keyEncriptacion.Length == 16)
                                        aes.KeySize = 128;
                                    else
                                        aes.KeySize = 256;
                                    aes.Mode = CipherMode.ECB;
                                    aes.Padding = PaddingMode.PKCS7;

                                    ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.ASCII.GetBytes(keyEncriptacion), null);
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        using (CryptoStream cryptoEncrypt = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                        {
                                            using (StreamWriter streamWriter = new StreamWriter(cryptoEncrypt))
                                            {
                                                streamWriter.Write(textoEncriptar);
                                            }
                                            textoEncriptado = memoryStream.ToArray();
                                            cadenaEncriptada = Convert.ToBase64String(textoEncriptado);
                                        }
                                    }
                                    Console.WriteLine("\nEl resultado de la encriptación del texto \"" + textoEncriptar + "\" es:");
                                    Console.WriteLine(cadenaEncriptada);
                                    Console.ReadKey();
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error al cifrar el texto. " + e.Message);
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("La llave no cumple la longitud de 16 o 32 caracteres, verifique y vuelva a intentar!");
                            Console.ReadKey();
                        }
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Ingrese la cadena a desencriptar: ");
                        string textoDesencriptar = Console.ReadLine();
                        Console.WriteLine("Ingrese la llave de desencriptación, debe ser de longitud 16 o 32: ");
                        byte[] keyDesencriptacion = Encoding.ASCII.GetBytes(Console.ReadLine());
                        if (keyDesencriptacion.Length == 16 || keyDesencriptacion.Length == 32)
                        {
                            try
                            {
                                //var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(textoDesencriptar);
                                //var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(keySize / 8).Take(keySize / 8).ToArray();
                                string textoDesencriptado = null;
                                using (Aes aes = Aes.Create())
                                {
                                    if (keyDesencriptacion.Length == 16)
                                        aes.KeySize = 128;
                                    else
                                        aes.KeySize = 256;
                                    aes.Mode = CipherMode.ECB;
                                    aes.Padding = PaddingMode.PKCS7;

                                    ICryptoTransform decryptor = aes.CreateDecryptor(keyDesencriptacion, null);
                                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(textoDesencriptar)))
                                    {
                                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                                        {
                                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                            {
                                                // Read the decrypted bytes from the decrypting stream
                                                // and place them in a string.
                                                textoDesencriptado = srDecrypt.ReadToEnd();
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine("\nEl resultado desencriptar el texto \"" + textoDesencriptar + "\" es:");
                                Console.WriteLine(textoDesencriptado);
                                Console.ReadKey();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error al descifrar respuesta. " + e.Message);
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("La llave no cumple la longitud de 16 o 32 caracteres, por favor verifique y vuelva a intentar!");
                            Console.ReadKey();
                        }
                        break;
                    case "3":
                        Console.WriteLine("\nGracias por usar el sistema!\n");
                        break;
                    default:
                        Console.WriteLine("Opción no válida, por favor verifique e intente nuevamente!");
                        break;
                }
            } while (opcionMenu != "3");
        }
    }
}
