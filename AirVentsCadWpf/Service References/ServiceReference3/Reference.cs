﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirVentsCadWpf.ServiceReference3 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://speller.yandex.net/services/spellservice", ConfigurationName="ServiceReference3.SpellServiceSoap")]
    public interface SpellServiceSoap {
        
        // CODEGEN: Generating message contract since the wrapper name (CheckTextRequest) of message checkTextRequest does not match the default value (checkText)
        [System.ServiceModel.OperationContractAttribute(Action="http://speller.yandex.net/services/spellservice/checkText", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AirVentsCadWpf.ServiceReference3.checkTextResponse checkText(AirVentsCadWpf.ServiceReference3.checkTextRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://speller.yandex.net/services/spellservice/checkText", ReplyAction="*")]
        System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference3.checkTextResponse> checkTextAsync(AirVentsCadWpf.ServiceReference3.checkTextRequest request);
        
        // CODEGEN: Generating message contract since the wrapper name (CheckTextsRequest) of message checkTextsRequest does not match the default value (checkTexts)
        [System.ServiceModel.OperationContractAttribute(Action="http://speller.yandex.net/services/spellservice/checkTexts", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AirVentsCadWpf.ServiceReference3.checkTextsResponse checkTexts(AirVentsCadWpf.ServiceReference3.checkTextsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://speller.yandex.net/services/spellservice/checkTexts", ReplyAction="*")]
        System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference3.checkTextsResponse> checkTextsAsync(AirVentsCadWpf.ServiceReference3.checkTextsRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://speller.yandex.net/services/spellservice")]
    public partial class SpellError : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string wordField;
        
        private string[] sField;
        
        private int codeField;
        
        private int posField;
        
        private int rowField;
        
        private int colField;
        
        private int lenField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string word {
            get {
                return this.wordField;
            }
            set {
                this.wordField = value;
                this.RaisePropertyChanged("word");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("s", Order=1)]
        public string[] s {
            get {
                return this.sField;
            }
            set {
                this.sField = value;
                this.RaisePropertyChanged("s");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
                this.RaisePropertyChanged("code");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int pos {
            get {
                return this.posField;
            }
            set {
                this.posField = value;
                this.RaisePropertyChanged("pos");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int row {
            get {
                return this.rowField;
            }
            set {
                this.rowField = value;
                this.RaisePropertyChanged("row");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int col {
            get {
                return this.colField;
            }
            set {
                this.colField = value;
                this.RaisePropertyChanged("col");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int len {
            get {
                return this.lenField;
            }
            set {
                this.lenField = value;
                this.RaisePropertyChanged("len");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CheckTextRequest", WrapperNamespace = "http://speller.yandex.net/services/spellservice", IsWrapped = true)]
    public partial class checkTextRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=0)]
        public string text;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=1)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lang;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=2)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int options;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=3)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string format;
        
        public checkTextRequest() {
        }
        
        public checkTextRequest(string text, string lang, int options, string format) {
            this.text = text;
            this.lang = lang;
            this.options = options;
            this.format = format;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CheckTextResponse", WrapperNamespace = "http://speller.yandex.net/services/spellservice", IsWrapped = true)]
    public partial class checkTextResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("error", IsNullable=false)]
        public AirVentsCadWpf.ServiceReference3.SpellError[] SpellResult;
        
        public checkTextResponse() {
        }
        
        public checkTextResponse(AirVentsCadWpf.ServiceReference3.SpellError[] SpellResult) {
            this.SpellResult = SpellResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CheckTextsRequest", WrapperNamespace = "http://speller.yandex.net/services/spellservice", IsWrapped = true)]
    public partial class checkTextsRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("text")]
        public string[] text;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=1)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lang;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=2)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int options;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=3)]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string format;
        
        public checkTextsRequest() {
        }
        
        public checkTextsRequest(string[] text, string lang, int options, string format) {
            this.text = text;
            this.lang = lang;
            this.options = options;
            this.format = format;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "CheckTextsResponse", WrapperNamespace = "http://speller.yandex.net/services/spellservice", IsWrapped = true)]
    public partial class checkTextsResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://speller.yandex.net/services/spellservice", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SpellResult", IsNullable=false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("error", IsNullable=false, NestingLevel=1)]
        public AirVentsCadWpf.ServiceReference3.SpellError[][] ArrayOfSpellResult;
        
        public checkTextsResponse() {
        }
        
        public checkTextsResponse(AirVentsCadWpf.ServiceReference3.SpellError[][] ArrayOfSpellResult) {
            this.ArrayOfSpellResult = ArrayOfSpellResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SpellServiceSoapChannel : AirVentsCadWpf.ServiceReference3.SpellServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SpellServiceSoapClient : System.ServiceModel.ClientBase<AirVentsCadWpf.ServiceReference3.SpellServiceSoap>, AirVentsCadWpf.ServiceReference3.SpellServiceSoap {
        
        public SpellServiceSoapClient() {
        }
        
        public SpellServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SpellServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SpellServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SpellServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AirVentsCadWpf.ServiceReference3.checkTextResponse AirVentsCadWpf.ServiceReference3.SpellServiceSoap.checkText(AirVentsCadWpf.ServiceReference3.checkTextRequest request) {
            return base.Channel.checkText(request);
        }
        
        public AirVentsCadWpf.ServiceReference3.SpellError[] checkText(string text, string lang, int options, string format) {
            AirVentsCadWpf.ServiceReference3.checkTextRequest inValue = new AirVentsCadWpf.ServiceReference3.checkTextRequest();
            inValue.text = text;
            inValue.lang = lang;
            inValue.options = options;
            inValue.format = format;
            AirVentsCadWpf.ServiceReference3.checkTextResponse retVal = ((AirVentsCadWpf.ServiceReference3.SpellServiceSoap)(this)).checkText(inValue);
            return retVal.SpellResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference3.checkTextResponse> AirVentsCadWpf.ServiceReference3.SpellServiceSoap.checkTextAsync(AirVentsCadWpf.ServiceReference3.checkTextRequest request) {
            return base.Channel.checkTextAsync(request);
        }
        
        public System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference3.checkTextResponse> checkTextAsync(string text, string lang, int options, string format) {
            AirVentsCadWpf.ServiceReference3.checkTextRequest inValue = new AirVentsCadWpf.ServiceReference3.checkTextRequest();
            inValue.text = text;
            inValue.lang = lang;
            inValue.options = options;
            inValue.format = format;
            return ((AirVentsCadWpf.ServiceReference3.SpellServiceSoap)(this)).checkTextAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AirVentsCadWpf.ServiceReference3.checkTextsResponse AirVentsCadWpf.ServiceReference3.SpellServiceSoap.checkTexts(AirVentsCadWpf.ServiceReference3.checkTextsRequest request) {
            return base.Channel.checkTexts(request);
        }
        
        public AirVentsCadWpf.ServiceReference3.SpellError[][] checkTexts(string[] text, string lang, int options, string format) {
            AirVentsCadWpf.ServiceReference3.checkTextsRequest inValue = new AirVentsCadWpf.ServiceReference3.checkTextsRequest();
            inValue.text = text;
            inValue.lang = lang;
            inValue.options = options;
            inValue.format = format;
            AirVentsCadWpf.ServiceReference3.checkTextsResponse retVal = ((AirVentsCadWpf.ServiceReference3.SpellServiceSoap)(this)).checkTexts(inValue);
            return retVal.ArrayOfSpellResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference3.checkTextsResponse> AirVentsCadWpf.ServiceReference3.SpellServiceSoap.checkTextsAsync(AirVentsCadWpf.ServiceReference3.checkTextsRequest request) {
            return base.Channel.checkTextsAsync(request);
        }
        
        public System.Threading.Tasks.Task<AirVentsCadWpf.ServiceReference3.checkTextsResponse> checkTextsAsync(string[] text, string lang, int options, string format) {
            AirVentsCadWpf.ServiceReference3.checkTextsRequest inValue = new AirVentsCadWpf.ServiceReference3.checkTextsRequest();
            inValue.text = text;
            inValue.lang = lang;
            inValue.options = options;
            inValue.format = format;
            return ((AirVentsCadWpf.ServiceReference3.SpellServiceSoap)(this)).checkTextsAsync(inValue);
        }
    }
}
