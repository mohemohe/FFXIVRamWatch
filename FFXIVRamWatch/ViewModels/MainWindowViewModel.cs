using FFXIVRamWatch.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using System.ComponentModel;
using System.Windows;

namespace FFXIVRamWatch.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ
         *
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *
         * を使用してください。
         *
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         *
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         *
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         *
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         *
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         *
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        private Model _model;

        public void Initialize()
        {
            _model = new Model();

            var modelListener = new PropertyChangedEventListener(_model);
            modelListener.RegisterHandler(ModelUpdateHandler);
            CompositeDisposable.Add(modelListener);

            _model.StartWatching();
        }

        private void ModelUpdateHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "FFXIVchecked":
                    var worker = sender as Model;
                    if (worker != null)
                    {
                        ProcessId = worker.checkFfxiv.ProcessId;
                        PrivateBytes = worker.checkFfxiv.PrivateBytesInfomation.BytesStr;
                        PrivateBytesColor = worker.checkFfxiv.PrivateBytesInfomation.Color;
                        VirtualBytes = worker.checkFfxiv.VirtualBytesInfomation.BytesStr;
                        VirtualBytesColor = worker.checkFfxiv.VirtualBytesInfomation.Color;
                    }
                    break;
            }
        }

        public void Closing()
        {
            Config.WindowPos = new Point(Application.Current.MainWindow.Left, Application.Current.MainWindow.Top);
        }

        #region CloseWindowCommand

        private ViewModelCommand _CloseWindowCommand;

        public ViewModelCommand CloseWindowCommand
        {
            get
            {
                if (_CloseWindowCommand == null)
                {
                    _CloseWindowCommand = new ViewModelCommand(CloseWindow);
                }
                return _CloseWindowCommand;
            }
        }

        public void CloseWindow()
        {
            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowState"));
        }

        #endregion CloseWindowCommand

        #region MinimizeWindowCommand

        private ViewModelCommand _MinimizeWindowCommand;

        public ViewModelCommand MinimizeWindowCommand
        {
            get
            {
                if (_MinimizeWindowCommand == null)
                {
                    _MinimizeWindowCommand = new ViewModelCommand(MinimizeWindow);
                }
                return _MinimizeWindowCommand;
            }
        }

        public void MinimizeWindow()
        {
            Messenger.Raise(new WindowActionMessage(WindowAction.Minimize, "WindowState"));
        }

        #endregion MinimizeWindowCommand

        #region ProcessId変更通知プロパティ

        private string _ProcessId = "searching";

        public string ProcessId
        {
            get { return _ProcessId; }
            set
            {
                if (_ProcessId == value)
                    return;
                _ProcessId = value;
                RaisePropertyChanged();
            }
        }

        #endregion ProcessId変更通知プロパティ

        #region PrivateBytes変更通知プロパティ

        private string _PrivateBytes = "-";

        public string PrivateBytes
        {
            get { return _PrivateBytes + " MB"; }
            set
            {
                if (_PrivateBytes == value)
                    return;
                _PrivateBytes = value;
                RaisePropertyChanged();
            }
        }

        #endregion PrivateBytes変更通知プロパティ

        #region PrivateBytesColor変更通知プロパティ

        private string _PrivateBytesColor = "#FFFFFFFF";

        public string PrivateBytesColor
        {
            get { return _PrivateBytesColor; }
            set
            {
                if (_PrivateBytesColor == value)
                    return;
                _PrivateBytesColor = value;
                RaisePropertyChanged();
            }
        }

        #endregion PrivateBytesColor変更通知プロパティ

        #region VirtualBytes変更通知プロパティ

        private string _VirtualBytes = "-";

        public string VirtualBytes
        {
            get { return _VirtualBytes + " MB"; }
            set
            {
                if (_VirtualBytes == value)
                    return;
                _VirtualBytes = value;
                RaisePropertyChanged();
            }
        }

        #endregion VirtualBytes変更通知プロパティ

        #region VirtualBytesColor変更通知プロパティ

        private string _VirtualBytesColor = "#FFFFFFFF";

        public string VirtualBytesColor
        {
            get { return _VirtualBytesColor; }
            set
            {
                if (_VirtualBytesColor == value)
                    return;
                _VirtualBytesColor = value;
                RaisePropertyChanged();
            }
        }

        #endregion VirtualBytesColor変更通知プロパティ
    }
}