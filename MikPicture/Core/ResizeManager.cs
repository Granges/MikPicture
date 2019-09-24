using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Granges.MikPicture.Core
{
    public class ResizeManager
    {
        private readonly Dispatcher dispatcher = new Dispatcher();

        #region Properties

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>The quality.</value>
        public long Quality { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>The source path.</value>
        public string SourcePath { get; set; }

        #endregion Properties

        #region Event

        public event EventHandler<ResizeEventArgs> Resizing;

        public event EventHandler Ended;

        private void OnResizing(string filename)
        {
            Resizing?.Invoke(this, new ResizeEventArgs(filename));
        }

        private void OnEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }

        #endregion Event

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeManager"/> class.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="quality">The quality.</param>
        public ResizeManager(string sourcePath, long quality)
        {
            Quality = quality;
            SourcePath = sourcePath;
        }

        #endregion ctor

        /// <summary>
        /// Gets the pictures files.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetPicturesFiles()
        {
            return Directory.GetFiles(SourcePath)
                .Where(f => IsAllowedExtension(f));
        }

        /// <summary>
        /// Determines whether [is allowed extension] [the specified path].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// 	<c>true</c> if [is allowed extension] [the specified path]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsAllowedExtension(string path)
        {
            return (new string[] { ".jpg", ".jpeg" })
                .Contains(Path.GetExtension(path).ToLower());
        }

        /// <summary>
        /// Gets the encoder.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders()
                .FirstOrDefault(c => c.FormatID == format.Guid);
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            string destPath = Path.Combine(SourcePath, "Converted");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            IEnumerable<string> files = GetPicturesFiles();

            Parallel.ForEach(files, sourceFilename =>
            {
                FileInfo fi = new FileInfo(sourceFilename);
                if (fi.Length > 1000000)
                {
                    //OnResizing(sourceFilename);

                    dispatcher.Invoke(new Action<string>(OnResizing), new object[] { sourceFilename });

                    // Length > 1Mo
                    Bitmap img = new Bitmap(sourceFilename);

                    string destFilename = Path.Combine(destPath, Path.GetFileName(sourceFilename));

                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, Quality);

                    img.Save(destFilename, jgpEncoder, myEncoderParameters);

                    img.Dispose();
                }
            });

            OnEnded();
        }
    }
}