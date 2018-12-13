using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPrint4PC_WPF
{
  public  class FileData:INotifyPropertyChanged
    {
        private String _FileFullName;
            
        private String _File_Name;
        private String _File_Size;
        private String _File_Type;


        public FileData(String FileFullName)
        {
            this._FileFullName = FileFullName;
            this._File_Name = FileFullName;
        }

        public String FileFullname
        {
            get { return _FileFullName; }

            set {

                _FileFullName =value;
                if(PropertyChanged!=null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("FileFullname"));
            }
        }
        public String FileName
        {
            get { return _File_Name; }

            set {
                _File_Name = value;
                if (PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("FileName"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(FileData))
                return false;
            FileData c = obj as FileData;
            return (this._FileFullName.Equals(c.FileFullname) );
        }
        public override int GetHashCode()
        {
            String hashString = this._FileFullName;
            return hashString.GetHashCode();
        }
        /**/

        public String Type
        {
            get { return ""; }
            set { }
        }


        public String Time
        {
            set { }
            get { return ""; }

        }

        public String Size
        {
            set{}
            get{ return ""; }    
        }
        /**/
    }
}
