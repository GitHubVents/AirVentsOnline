﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirVentsCadWpf.ServiceReference2 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BomPartListClass.BomCells", Namespace="http://schemas.datacontract.org/2004/07/BomPartList")]
    [System.SerializableAttribute()]
    public partial class BomPartListClassBomCells : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaterialDocMgrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string АссоциированныйОбъектField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string КоличествоField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string КонфигурацияField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string МатериалField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string МатериалЦмиField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string МатериалЦмиDocMgrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string НаименованиеField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string НаименованиеDocMgrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ОбозначениеField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ОбозначениеDocMgrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ПоследняяВерсияField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ПутьField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string РазделField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string РазделDocMgrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ТипФайлаField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ТолщинаЛистаField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ТолщинаЛистаDocMgrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string УровеньField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Errors {
            get {
                return this.ErrorsField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorsField, value) != true)) {
                    this.ErrorsField = value;
                    this.RaisePropertyChanged("Errors");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MaterialDocMgr {
            get {
                return this.MaterialDocMgrField;
            }
            set {
                if ((object.ReferenceEquals(this.MaterialDocMgrField, value) != true)) {
                    this.MaterialDocMgrField = value;
                    this.RaisePropertyChanged("MaterialDocMgr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string АссоциированныйОбъект {
            get {
                return this.АссоциированныйОбъектField;
            }
            set {
                if ((object.ReferenceEquals(this.АссоциированныйОбъектField, value) != true)) {
                    this.АссоциированныйОбъектField = value;
                    this.RaisePropertyChanged("АссоциированныйОбъект");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Количество {
            get {
                return this.КоличествоField;
            }
            set {
                if ((object.ReferenceEquals(this.КоличествоField, value) != true)) {
                    this.КоличествоField = value;
                    this.RaisePropertyChanged("Количество");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Конфигурация {
            get {
                return this.КонфигурацияField;
            }
            set {
                if ((object.ReferenceEquals(this.КонфигурацияField, value) != true)) {
                    this.КонфигурацияField = value;
                    this.RaisePropertyChanged("Конфигурация");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Материал {
            get {
                return this.МатериалField;
            }
            set {
                if ((object.ReferenceEquals(this.МатериалField, value) != true)) {
                    this.МатериалField = value;
                    this.RaisePropertyChanged("Материал");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string МатериалЦми {
            get {
                return this.МатериалЦмиField;
            }
            set {
                if ((object.ReferenceEquals(this.МатериалЦмиField, value) != true)) {
                    this.МатериалЦмиField = value;
                    this.RaisePropertyChanged("МатериалЦми");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string МатериалЦмиDocMgr {
            get {
                return this.МатериалЦмиDocMgrField;
            }
            set {
                if ((object.ReferenceEquals(this.МатериалЦмиDocMgrField, value) != true)) {
                    this.МатериалЦмиDocMgrField = value;
                    this.RaisePropertyChanged("МатериалЦмиDocMgr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Наименование {
            get {
                return this.НаименованиеField;
            }
            set {
                if ((object.ReferenceEquals(this.НаименованиеField, value) != true)) {
                    this.НаименованиеField = value;
                    this.RaisePropertyChanged("Наименование");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string НаименованиеDocMgr {
            get {
                return this.НаименованиеDocMgrField;
            }
            set {
                if ((object.ReferenceEquals(this.НаименованиеDocMgrField, value) != true)) {
                    this.НаименованиеDocMgrField = value;
                    this.RaisePropertyChanged("НаименованиеDocMgr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Обозначение {
            get {
                return this.ОбозначениеField;
            }
            set {
                if ((object.ReferenceEquals(this.ОбозначениеField, value) != true)) {
                    this.ОбозначениеField = value;
                    this.RaisePropertyChanged("Обозначение");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ОбозначениеDocMgr {
            get {
                return this.ОбозначениеDocMgrField;
            }
            set {
                if ((object.ReferenceEquals(this.ОбозначениеDocMgrField, value) != true)) {
                    this.ОбозначениеDocMgrField = value;
                    this.RaisePropertyChanged("ОбозначениеDocMgr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ПоследняяВерсия {
            get {
                return this.ПоследняяВерсияField;
            }
            set {
                if ((object.ReferenceEquals(this.ПоследняяВерсияField, value) != true)) {
                    this.ПоследняяВерсияField = value;
                    this.RaisePropertyChanged("ПоследняяВерсия");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Путь {
            get {
                return this.ПутьField;
            }
            set {
                if ((object.ReferenceEquals(this.ПутьField, value) != true)) {
                    this.ПутьField = value;
                    this.RaisePropertyChanged("Путь");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Раздел {
            get {
                return this.РазделField;
            }
            set {
                if ((object.ReferenceEquals(this.РазделField, value) != true)) {
                    this.РазделField = value;
                    this.RaisePropertyChanged("Раздел");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string РазделDocMgr {
            get {
                return this.РазделDocMgrField;
            }
            set {
                if ((object.ReferenceEquals(this.РазделDocMgrField, value) != true)) {
                    this.РазделDocMgrField = value;
                    this.RaisePropertyChanged("РазделDocMgr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ТипФайла {
            get {
                return this.ТипФайлаField;
            }
            set {
                if ((object.ReferenceEquals(this.ТипФайлаField, value) != true)) {
                    this.ТипФайлаField = value;
                    this.RaisePropertyChanged("ТипФайла");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ТолщинаЛиста {
            get {
                return this.ТолщинаЛистаField;
            }
            set {
                if ((object.ReferenceEquals(this.ТолщинаЛистаField, value) != true)) {
                    this.ТолщинаЛистаField = value;
                    this.RaisePropertyChanged("ТолщинаЛиста");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ТолщинаЛистаDocMgr {
            get {
                return this.ТолщинаЛистаDocMgrField;
            }
            set {
                if ((object.ReferenceEquals(this.ТолщинаЛистаDocMgrField, value) != true)) {
                    this.ТолщинаЛистаDocMgrField = value;
                    this.RaisePropertyChanged("ТолщинаЛистаDocMgr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Уровень {
            get {
                return this.УровеньField;
            }
            set {
                if ((object.ReferenceEquals(this.УровеньField, value) != true)) {
                    this.УровеньField = value;
                    this.RaisePropertyChanged("Уровень");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference2.IBomTableService")]
    public interface IBomTableService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBomTableService/SayHello", ReplyAction="http://tempuri.org/IBomTableService/SayHelloResponse")]
        string SayHello(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBomTableService/SayHello", ReplyAction="http://tempuri.org/IBomTableService/SayHelloResponse")]
        System.Threading.Tasks.Task<string> SayHelloAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBomTableService/BomParts", ReplyAction="http://tempuri.org/IBomTableService/BomPartsResponse")]
        AirVentsCadWpf.ServiceReference2.BomPartListClassBomCells[] BomParts(string assemblyPath, string pdmBaseName, string userName, string password, string constring);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBomTableService/BomParts", ReplyAction="http://tempuri.org/IBomTableService/BomPartsResponse")]
        System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference2.BomPartListClassBomCells[]> BomPartsAsync(string assemblyPath, string pdmBaseName, string userName, string password, string constring);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBomTableServiceChannel : AirVentsCadWpf.ServiceReference2.IBomTableService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BomTableServiceClient : System.ServiceModel.ClientBase<AirVentsCadWpf.ServiceReference2.IBomTableService>, AirVentsCadWpf.ServiceReference2.IBomTableService {
        
        public BomTableServiceClient() {
        }
        
        public BomTableServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BomTableServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BomTableServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BomTableServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string SayHello(string name) {
            return base.Channel.SayHello(name);
        }
        
        public System.Threading.Tasks.Task<string> SayHelloAsync(string name) {
            return base.Channel.SayHelloAsync(name);
        }
        
        public AirVentsCadWpf.ServiceReference2.BomPartListClassBomCells[] BomParts(string assemblyPath, string pdmBaseName, string userName, string password, string constring) {
            return base.Channel.BomParts(assemblyPath, pdmBaseName, userName, password, constring);
        }
        
        public System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference2.BomPartListClassBomCells[]> BomPartsAsync(string assemblyPath, string pdmBaseName, string userName, string password, string constring) {
            return base.Channel.BomPartsAsync(assemblyPath, pdmBaseName, userName, password, constring);
        }
    }
}
