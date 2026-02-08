using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace DarwinXNA
{

    public class XNAFont : IDisposable
    {
        string strFontFamily;
        Texture2D texture;
        SpriteBatch fontRenderer;
        public int Height;
        public int Kerning = 0;


        struct XNAFontUV
        {
            public float u, v;
            public int Width;

            public XNAFontUV(float u, float v, int Width)
            {
                this.u = u;
                this.v = v;
                this.Width = Width;
            }
        }

        Dictionary<char, XNAFontUV> UVData = new Dictionary<char, XNAFontUV>();

        public void OutputText(string str, int x, int y, Color col)
        {
            int iCurrentX = x;
            int iCurrentY = y;

            foreach (char c in str)
            {
                XNAFontUV uv = UVData[c];

                if (c != ' ')
                {
                    fontRenderer.Draw(texture,
                        new Rectangle(iCurrentX, iCurrentY,
                            uv.Width, Height),
                        new Rectangle((int)uv.v,
                                      (int)uv.u,
                                      uv.Width, Height),
                        col);
                }

                iCurrentX += uv.Width - Kerning;
            }
        }

        public static XNAFont LoadFont(GraphicsDevice device, string strXML, string strDDS, SpriteBatch unSpriteBatch)
        {
            XNAFont ret = new XNAFont();

            System.Xml.XmlTextReader tr = new XmlTextReader(strXML);
            while (tr.Read())
            {
                if (tr.Name == "Font")
                {
                    if (tr.AttributeCount > 0)
                    {
                        ret.strFontFamily = tr.GetAttribute("Family");
                        ret.Height = Convert.ToInt32(tr.GetAttribute("Height"));

                    }
                }
                else if (tr.Name == "Char")
                {
                    string str = tr.GetAttribute("c");
                    str = str.Replace("&qwo;", "\"");
                    str = str.Replace("&amp;", "&");
                    str = str.Replace("&lt;", "<");
                    str = str.Replace("&gt;", ">");
                    char c = str[0];

                    ret.UVData.Add(c,
                        new XNAFontUV(
                            Convert.ToSingle(tr.GetAttribute("u")),
                            Convert.ToSingle(tr.GetAttribute("v")),
                            Convert.ToInt32(tr.GetAttribute("width"))));

                }
            }
            tr.Close();

            // MonoGame no soporta archivos DDS, intentar cargar PNG primero
            string pngPath = System.IO.Path.ChangeExtension(strDDS, ".png");
            
            if (System.IO.File.Exists(pngPath))
            {
                ret.texture = Texture2D.FromFile(device, pngPath);
            }
            else if (System.IO.File.Exists(strDDS))
            {
                throw new NotSupportedException(
                    $"MonoGame no soporta archivos DDS. Por favor convierte '{strDDS}' a PNG.\n" +
                    $"Ejecuta el script ConvertDDStoPNG.ps1 o convierte manualmente el archivo a PNG.");
            }
            else
            {
                throw new System.IO.FileNotFoundException(
                    $"No se encontró el archivo de fuente en PNG o DDS: {pngPath} o {strDDS}");
            }
            
            ret.fontRenderer = unSpriteBatch;

            return ret;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (texture != null)
                texture.Dispose();

            if (fontRenderer != null)
                fontRenderer.Dispose();

            texture = null;
            fontRenderer = null;
        }

        #endregion
    }
}
