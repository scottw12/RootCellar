using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// Summary description for EncryptDecrypt
/// </summary>
public class EncryptDecrypt
{
    private static string m_strPassPhrase = "MyPriv@Password!$$";
    private static string m_strHashAlgorithm = "MD5";
    private static int m_strPasswordIterations = 2;
    private static string m_strInitVector = "@1B2c3D4e5F6g7H8";
    private static int m_intKeySize = 256;

	public EncryptDecrypt()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public static string Encrypt(string pstrText)
    {
        string pstrEncrKey = "1239;[pewGKG)NisarFidesTech";
        byte[] byKey = { };
        byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
        byKey = System.Text.Encoding.UTF8.GetBytes(pstrEncrKey.Substring(0, 8));
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray = Encoding.UTF8.GetBytes(pstrText);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string pstrText)
    {
        pstrText = pstrText.Replace(" ", "+");
        string pstrDecrKey = "1239;[pewGKG)NisarFidesTech";
        byte[] byKey = { };
        byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
        byte[] inputByteArray = new byte[pstrText.Length];

        byKey = System.Text.Encoding.UTF8.GetBytes(pstrDecrKey.Substring(0, 8));
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        inputByteArray = Convert.FromBase64String(pstrText);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        return encoding.GetString(ms.ToArray());
    }
    static internal string EncryptPasswordMD5(string plainText, string p_strSaltValue)
    {
        string strReturn = string.Empty;

        try
        {
            byte[] initVectorBytes = null;
            initVectorBytes = System.Text.Encoding.ASCII.GetBytes(m_strInitVector);
            byte[] saltValueBytes = null;
            saltValueBytes = System.Text.Encoding.ASCII.GetBytes(p_strSaltValue);
            byte[] plainTextBytes = null;
            plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            Rfc2898DeriveBytes password = default(Rfc2898DeriveBytes);
            password = new Rfc2898DeriveBytes(m_strPassPhrase, saltValueBytes, m_strPasswordIterations);

            byte[] keyBytes = null;
            int intKeySize = 0;
            intKeySize = Convert.ToInt32((m_intKeySize / 8));
            keyBytes = password.GetBytes(intKeySize);

            System.Security.Cryptography.RijndaelManaged symmetricKey = null;
            symmetricKey = new System.Security.Cryptography.RijndaelManaged();

            symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;

            System.Security.Cryptography.ICryptoTransform encryptor = null;
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            System.IO.MemoryStream memoryStream = null;
            memoryStream = new System.IO.MemoryStream();

            System.Security.Cryptography.CryptoStream cryptoStream = null;
            cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, encryptor, System.Security.Cryptography.CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = null;
            cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = null;
            cipherText = Convert.ToBase64String(cipherTextBytes);

            strReturn = cipherText;
        }
        catch (Exception ex)
        {
            strReturn = null;
        }
        return strReturn;
    }
    public static string DecryptPasswordMD5(string cipherText, string p_strSaltValue)
    {
        string strReturn = string.Empty;

        try
        {
            byte[] initVectorBytes = null;
            initVectorBytes = System.Text.Encoding.ASCII.GetBytes(m_strInitVector);
            byte[] saltValueBytes = null;
            saltValueBytes = System.Text.Encoding.ASCII.GetBytes(p_strSaltValue);

            byte[] cipherTextBytes = null;
            cipherTextBytes = Convert.FromBase64String(cipherText.ToString());

            Rfc2898DeriveBytes password = default(Rfc2898DeriveBytes);
            password = new Rfc2898DeriveBytes(m_strPassPhrase, saltValueBytes, m_strPasswordIterations);
            //   
            byte[] keyBytes = null;
            int intKeySize = 0;
            //Integer
            intKeySize = Convert.ToInt32((m_intKeySize / 8));
            keyBytes = password.GetBytes(intKeySize);

            System.Security.Cryptography.RijndaelManaged symmetricKey = null;
            symmetricKey = new System.Security.Cryptography.RijndaelManaged();

            symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;

            System.Security.Cryptography.ICryptoTransform decryptor = null;
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            System.IO.MemoryStream memoryStream = null;
            memoryStream = new System.IO.MemoryStream(cipherTextBytes);

            System.Security.Cryptography.CryptoStream cryptoStream = null;
            cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, decryptor, System.Security.Cryptography.CryptoStreamMode.Read);

            byte[] plainTextBytes = null;
            plainTextBytes = new byte[cipherTextBytes.Length + 1];

            int decryptedByteCount = 0;
            decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string plainText = null;
            plainText = System.Text.Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            strReturn = plainText;
        }
        catch (Exception ex)
        {
            strReturn = null;
        }
        return strReturn;
    }


    public static string EncryptPassword(string Password)
    {
        string sEncryptedPassword = "";

        string sEncryptKey = "P@SSW@RD@09";
        //Should be minimum 8 characters     

        try
        {
            sEncryptedPassword = EncryptPasswordMD5(Password, sEncryptKey);


        }
        catch (Exception ex)
        {
            return sEncryptedPassword;
        }
        return sEncryptedPassword;
    }


    public static string DecryptPassword(string Password)
    {
        string sDecryptedPassword = "";
        string sEncryptKey = "P@SSW@RD@09";
        //Should be minimum 8 characters    
        try
        {
            sDecryptedPassword = DecryptPasswordMD5(Password, sEncryptKey);
        }
        catch (Exception ex)
        {
            return sDecryptedPassword;
        }
        return sDecryptedPassword;
    }



    public string CreatePatientId(string sPatientName, System.DateTime dob, string ssn)
    {
        string sPatientId = "";
        try
        {
            string[] arrPatientName = sPatientName.Split(' ');
            string sPatientFName = arrPatientName[0];
            // string sub = input.Substring(3);
            string sDOB = dob.ToString("yyyy/MM/dd");
            sDOB = sDOB.Replace("/", "");
            string sSSN = ssn.Substring(4, 7);
            sPatientId = sPatientFName + sDOB + sSSN;
        }
        catch (Exception ex)
        {
            //  ErrorLog("Error####:" + ex.ToString());

        }
        finally
        {

        }
        return sPatientId;
    }
    public string CreatePractiseId(string strPraticeName, string strNPI)
    {
        string strPractiseId = "";
        try
        {
            string[] arrPatientName = strPraticeName.Split(' ');
            string sPatientFName = arrPatientName[0];
            strPractiseId = sPatientFName + strNPI;
        }
        catch (Exception ex)
        {
            //  ErrorLog("Error####:" + ex.ToString());

        }
        finally
        {

        }
        return strPractiseId;
    }
}