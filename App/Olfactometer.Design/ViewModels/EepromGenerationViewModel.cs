using System.Net;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MessageBox.Avalonia.Enums;
using Olfactometer.Design.Models;

namespace Olfactometer.Design.ViewModels
{
    public partial class EepromGenerationViewModel : ReactiveObject
    {
        [Reactive] public string InputFileName { get; set; }
        [Reactive] public string OutputFileName { get; set; }

        [Reactive] public bool IsGenerated { get; set; }
        [Reactive] public int SerialNumber { get; set; }
        [Reactive] public int Temperature { get; set; }

        public Interaction<Unit, string?> ShowOpenFileDialog { get; }
        public Interaction<Unit, string?> ShowSaveFileDialog { get; }
        public ReactiveCommand<Unit, Unit> BrowseOpenCommand { get; }
        public ReactiveCommand<Unit, Unit> GenerateCommand { get; }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        private EepromManager _manager;

        public EepromGenerationViewModel()
        {
            ShowOpenFileDialog = new Interaction<Unit, string?>();
            ShowSaveFileDialog = new Interaction<Unit, string?>();
            BrowseOpenCommand = ReactiveCommand.CreateFromTask(OpenInputFileAsync);
            GenerateCommand = ReactiveCommand.CreateFromTask(GenerateAsync,
                this.WhenAnyValue(x => x.InputFileName).Select(x => !string.IsNullOrEmpty(x)));
            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync,
                this.WhenAnyValue(x => x.IsGenerated).Select(x => x));
        }

        private Task GenerateAsync()
        {
            if (string.IsNullOrWhiteSpace(InputFileName))
                // TODO: should present an error to the user
                return Task.CompletedTask;

            //TODO: should throw exception if there are errors in the file
            _manager = new EepromManager(InputFileName);
            _manager.ProcessFile();
            _manager.GenerateEeprom(SerialNumber, Temperature);

            IsGenerated = true;

            return Task.CompletedTask;
        }

        private async Task OpenInputFileAsync()
        {
            var fileName = await ShowOpenFileDialog.Handle(Unit.Default);
            if (fileName is object)
            {
                InputFileName = fileName;
            }
        }

        private async Task SaveAsync()
        {
            if (_manager == null)
                //TODO: should show error
                return;

            var fileName = await ShowSaveFileDialog.Handle(Unit.Default);
            if (fileName is object)
            {
                _manager.Save(fileName);

                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("File saved successfully",
                        "File saved successfully",
                        icon: Icon.Success);
                await messageBoxStandardWindow.Show();
            }
        }
    }
}
