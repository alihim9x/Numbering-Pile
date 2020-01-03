using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace SingleData
{
    public class FormData : NotifyClass
    {
        public static FormData Instance { get { return Singleton.Instance.FormData; } }
        private Model.Form.InputForm inputForm;
        public Model.Form.InputForm InputForm
        {
            get
            {
                if (inputForm == null)
                {
                    inputForm = new Model.Form.InputForm { DataContext = this };
                }
                return inputForm;
            }
        }
        //private Model.ViewModel.ElementView elementView;
        //public Model.ViewModel.ElementView ElementView
        //{
        //    get
        //    {
        //        return elementView;
        //    }
        //    set
        //    {
        //        elementView = value;
        //        OnPropertyChanged();
        //    }
        //}
        private Model.ViewModel.SettingView settingView;
        public Model.ViewModel.SettingView SettingView
        {
            get
            {
                if (settingView == null)
                {
                    settingView = new Model.ViewModel.SettingView()
                    {
                        Setting = ModelData.Instance.Setting
                    };
                }
                return settingView;
            }
        }

        //private ObservableCollection<Model.ViewModel.RebarView> rebarViews;
        //public ObservableCollection<Model.ViewModel.RebarView> RebarViews
        //{
        //    get
        //    {
        //        if(rebarViews == null)
        //        {
        //            rebarViews = new ObservableCollection<Model.ViewModel.RebarView>();
        //        }
        //        return rebarViews;
        //    }
        //    set
        //    {
        //        rebarViews = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private ObservableCollection<Model.ViewModel.StirrupView> stirrupViews;
        //public ObservableCollection<Model.ViewModel.StirrupView> StirrupViews
        //{
        //    get
        //    {
        //        if(stirrupViews == null)
        //        {
        //            stirrupViews = new ObservableCollection<Model.ViewModel.StirrupView>();
        //        }
        //        return stirrupViews;
        //    }
        //    set
        //    {
        //        stirrupViews = value;
        //        OnPropertyChanged();
        //    }
        //}

    }
}
