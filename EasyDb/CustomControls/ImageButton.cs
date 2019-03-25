namespace EasyDb.CustomControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="ImageButton" />
    /// </summary>
    public class ImageButton : Button
    {
        /// <summary>
        /// Defines the ImageProperty
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image",
            typeof(ImageSource),
            typeof(ImageButton),
            new PropertyMetadata(default(ImageSource)));

        /// <summary>
        /// Defines the TextProperty
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(ImageButton),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes static members of the <see cref="ImageButton"/> class.
        /// </summary>
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ImageButton),
                new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        /// <summary>
        /// Gets or sets the Image
        /// </summary>
        public ImageSource Image
        {
            get
            {
                return (ImageSource)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}