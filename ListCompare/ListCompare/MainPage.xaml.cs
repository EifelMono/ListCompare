using EifelMono.QuickButDirty.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ListCompare
{
    public partial class MainPage : ContentPage
    {
        public class BindingData : BindingClass
        {
            public BindingProperty<string> List1 { get; set; } = new BindingProperty<string>().Default("");
            public BindingProperty<string> List2 { get; set; } = new BindingProperty<string>().Default("");
            public BindingProperty<string> ListInBooth { get; set; } = new BindingProperty<string>().Default("");
            public BindingProperty<string> ListInList1Only { get; set; } = new BindingProperty<string>().Default("");
            public BindingProperty<string> ListInList2Only { get; set; } = new BindingProperty<string>().Default("");
            public BindingProperty<string> List1NotInList2 { get; set; } = new BindingProperty<string>().Default("");

            public BindingProperty<int> CountList1 { get; set; } = new BindingProperty<int>();
            public BindingProperty<int> CountList2 { get; set; } = new BindingProperty<int>();
            public BindingProperty<int> CountListInBooth { get; set; } = new BindingProperty<int>();
            public BindingProperty<int> CountInList1Only { get; set; } = new BindingProperty<int>();
            public BindingProperty<int> CountInList2Only { get; set; } = new BindingProperty<int>();
            public BindingProperty<int> CountList1NotInList2 { get; set; } = new BindingProperty<int>();

            ICommand _CommandCompare = null;

            public ICommand CommandCompare
            {
                get
                {
                    return _CommandCompare ?? (_CommandCompare = new Command(() =>
                    {
                        Clear(false);
                        CheckLineFeed(List1.Value, true);
                        CheckLineFeed(List2.Value);

                        var list1 = TextToList(List1.Value);
                        var list2 = TextToList(List2.Value);

                        var listInBooth = new List<string>();
                        var listInList1Only = new List<string>();
                        var listInList2Only = new List<string>();

                        var list1NotInList2 = new List<string>();

                        foreach (var item1 in list1)
                        {
                            if (list2.Contains(item1))
                                listInBooth.Add(item1);
                        }
                        ListInBooth.Value = ListToText(listInBooth);

                        foreach (var item1 in list1)
                        {
                            if (!list2.Contains(item1))
                                listInList1Only.Add(item1);
                        }
                        ListInList1Only.Value = ListToText(listInList1Only);

                        foreach (var item2 in list2)
                        {
                            if (!list1.Contains(item2))
                                listInList2Only.Add(item2);
                        }
                        ListInList2Only.Value = ListToText(listInList2Only);

                        CountList1.Value = list1.Count;
                        CountList2.Value = list2.Count;
                        CountListInBooth.Value = listInBooth.Count;
                        CountInList1Only.Value = listInList1Only.Count;
                        CountInList2Only.Value = listInList2Only.Count;

                        CountList1NotInList2.Value = list1NotInList2.Count;
                        RefreshAll();
                    }));
                }
            }

            ICommand _CommandClear = null;

            public ICommand CommandClear
            {
                get
                {
                    return _CommandClear ?? (_CommandClear = new Command(() =>
                    {
                        Clear(true);
                        RefreshAll();
                    }));
                }
            }

            void Clear(bool all)
            {
                if (all)
                {
                    List1.Value = "";
                    List2.Value = "";
                }
                ListInBooth.Value = "";
                ListInList1Only.Value = "";
                ListInList2Only.Value = "";
                List1NotInList2.Value = "";
            }

            string LineFeed = null;
            void CheckLineFeed(string text, bool init = false)
            {
                if (init)
                    LineFeed = null;
                if (!string.IsNullOrEmpty(text))
                    if (LineFeed == null)
                    {
                        foreach (var lf in new string[] { "\r\n", "\n\r", "\n", "r" })
                            if (text.Contains(lf))
                            {
                                LineFeed = lf;
                                break;
                            }
                        if (LineFeed == null)
                            LineFeed = " ";
                    }
            }

            const char Splitter = '\x01';
            List<string> TextToList(string text)
            {
                if (string.IsNullOrEmpty(text))
                    return new List<string>();
                var result = new List<string>();
                foreach (var t in text.Replace(LineFeed, $"{Splitter}").Split(Splitter))
                    if (!string.IsNullOrEmpty(t))
                        result.Add(t);
                return result;
            }

            string ListToText(List<string> list)
            {
                return string.Join(LineFeed, list);
            }
        }
        public BindingData Data { get; set; }
        public MainPage()
        {
            InitializeComponent();
            BindingContext = Data = new BindingData();
            Data.List1.Value = "1\r\n2\r\n";
            Data.List2.Value = "1\r\n2\r\n3\r\n";

        }
    }
}
