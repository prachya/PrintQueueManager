using Size = System.Windows.Size;



namespace ScaledPrintingWPF 
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for the main window
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Local storage to reach the view model from here
        /// </summary>
        private readonly MainWindowViewModel mainWindowViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.Loaded += this.MainWindowLoaded;
            this.mainWindowViewModel = (MainWindowViewModel)DataContext;
        }

        /// <summary>
        /// Handles the Loaded event of the MainWindow control.
        /// It is used to transfer information of the main form (like the dimensions
        /// of the preview box) the the view model after the is fully loaded.
        /// This information is only valid after the form is fully loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            // make a initial setup of the form here
            RadioButtonVerticalAlignmentCenter.IsChecked = true;
            RadioButtonHorizontalAlignmentCenter.IsChecked = true;
            RadioButtonOrientationPortrait.IsChecked = true;
            CheckBoxScaleToFit.IsChecked = true;
            CheckBoxUsePrintableArea.IsChecked = false;
            RadioButtonPagesAll.IsChecked = true;
            TextboxPagesRangeFrom.Text = @"from";
            TextboxPagesRangeTo.Text = @"to";
            labelDocumentName.Content = @"file name";
            LabelDocumentInfo.Content = @"Document:";
            var previewSize = new Size(viewboxPreview.Width, viewboxPreview.Height);
            mainWindowViewModel.PreviewSize = previewSize;
            mainWindowViewModel.ViewIsLoaded = true;
        }

        /// <summary>
        /// Relays the user interface action "menu->File->Open" to the view model
        /// </summary>
        /// <param name="sender">The object sending this event</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void MenuFileOpen(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.UserActionFileOpen();
        }

        /// <summary>
        /// Relay the user interface action "button [print] -> pressed" to the view model
        /// </summary>
        /// <param name="sender">The object sending this event</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void ButtonPrintPressed(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.UserActionStartPrint();
        }

        /// <summary>
        /// Relays the user interface action "button [cancel] -> pressed" to the view model
        /// </summary>
        /// <param name="sender">The object sending this event</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void ButtonCancelPressed(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.UserActionCancel();
        }

        /// <summary>
        /// Relays the user interface action "button [previous page] -> pressed" to the view model
        /// </summary>
        /// <param name="sender">The object sending this event</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void ButtonPreviousPage(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.UserActionPreviousPage();
        }

        /// <summary>
        /// Relays the user interface action "button [next page] -> pressed" to the view model
        /// </summary>
        /// <param name="sender">The object sending this event</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void ButtonNextPage(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.UserActionNextPage();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
