namespace wphTweaks.Regedit
{
    using System;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using System.Windows.Resources;


    public class IconStore
    {

        private static BitmapImage iconZip;

        private static BitmapImage iconXMLDocument;

        private static BitmapImage iconWordDocument;

        private static BitmapImage iconVideo;

        private static BitmapImage iconTextDocument;

        private static BitmapImage iconSystem;

        private static BitmapImage iconSound;

        private static BitmapImage iconPowerPointDocument;

        private static BitmapImage iconPDFDocument;

        private static BitmapImage iconImage;

        private static BitmapImage iconHTMLDocument;

        private static BitmapImage iconFolder;

        private static BitmapImage iconFile;

        private static BitmapImage iconExcelDocument;

        private static BitmapImage iconCertificate;
        
        public static BitmapImage LoadIcon(string iconName)
        {
            //Uri uri = new Uri("/Resources/Images/icon_{0}.png", UriKind.Relative); // Content 
            //Uri uri = new Uri("/Yaaf.Wp7.AdvancedExplorer;component/Images/icon_{0}.png", UriKind.Relative); // Resource
            var uri = new Uri(
                string.Format("/Regedit/Images/icon_{0}.png", iconName.ToLower()), UriKind.Relative);
            return new BitmapImage(uri);
            //StreamResourceInfo resourceStream = Application.GetResourceStream(uri);
            //using (resourceStream.Stream)
            //{
            //    BitmapImage image = new BitmapImage();
            //    image.SetSource(resourceStream.Stream);
            //    return image;
            //}
        }



        public static BitmapImage IconCertificate
        {
            get
            {
                if (iconCertificate == null)
                {
                    iconCertificate = LoadIcon("Certificate");
                }
                return iconCertificate;
            }
        }

        public static BitmapImage IconExcelDocument
        {
            get
            {
                if (iconExcelDocument == null)
                {
                    iconExcelDocument = LoadIcon("ExcelDocument");
                }
                return iconExcelDocument;
            }
        }

        public static BitmapImage IconFile
        {
            get
            {
                if (iconFile == null)
                {
                    iconFile = LoadIcon("File");
                }
                return iconFile;
            }
        }

        public static BitmapImage IconFolder
        {
            get
            {
                if (iconFolder == null)
                {
                    iconFolder = LoadIcon("Folder");
                }
                return iconFolder;
            }
        }

        public static BitmapImage IconHTMLDocument
        {
            get
            {
                if (iconHTMLDocument == null)
                {
                    iconHTMLDocument = LoadIcon("HTMLDocument");
                }
                return iconHTMLDocument;
            }
        }

        public static BitmapImage IconImage
        {
            get
            {
                if (iconImage == null)
                {
                    iconImage = LoadIcon("Image");
                }
                return iconImage;
            }
        }

        public static BitmapImage IconPDFDocument
        {
            get
            {
                if (iconPDFDocument == null)
                {
                    iconPDFDocument = LoadIcon("PDFDocument");
                }
                return iconPDFDocument;
            }
        }

        public static BitmapImage IconPowerPointDocument
        {
            get
            {
                if (iconPowerPointDocument == null)
                {
                    iconPowerPointDocument = LoadIcon("PowerPointDocument");
                }
                return iconPowerPointDocument;
            }
        }

        public static BitmapImage IconSound
        {
            get
            {
                if (iconSound == null)
                {
                    iconSound = LoadIcon("Sound");
                }
                return iconSound;
            }
        }

        public static BitmapImage IconSystem
        {
            get
            {
                if (iconSystem == null)
                {
                    iconSystem = LoadIcon("System");
                }
                return iconSystem;
            }
        }

        public static BitmapImage IconTextDocument
        {
            get
            {
                if (iconTextDocument == null)
                {
                    iconTextDocument = LoadIcon("TextDocument");
                }
                return iconTextDocument;
            }
        }

        public static BitmapImage IconVideo
        {
            get
            {
                if (iconVideo == null)
                {
                    iconVideo = LoadIcon("Video");
                }
                return iconVideo;
            }
        }

        public static BitmapImage IconWordDocument
        {
            get
            {
                if (iconWordDocument == null)
                {
                    iconWordDocument = LoadIcon("WordDocument");
                }
                return iconWordDocument;
            }
        }

        public static BitmapImage IconXMLDocument
        {
            get
            {
                if (iconXMLDocument == null)
                {
                    iconXMLDocument = LoadIcon("XMLDocument");
                }
                return iconXMLDocument;
            }
        }

        public static BitmapImage IconZip
        {
            get
            {
                if (iconZip == null)
                {
                    iconZip = LoadIcon("Zip");
                }
                return iconZip;
            }
        }
    }
}