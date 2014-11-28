//>>built
require({cache:{"url:epi-cms/contentediting/editors/templates/ContentAreaEditor.html":"<div class=\"dijitInline\">\r\n    <div data-dojo-attach-point=\"treeNode\"></div>\r\n    <div data-dojo-attach-point=\"actionsContainer\" class=\"epi-content-area-actionscontainer\"></div>            \r\n</div>"}});define("epi-cms/contentediting/editors/ContentAreaEditor",["dojo/_base/declare","dojo/_base/lang","dojo/aspect","dojo/dom-style","dojo/Deferred","dojo/on","dojo/topic","dojo/when","dijit/registry","dijit/_WidgetBase","dijit/_TemplatedMixin","dijit/_CssStateMixin","dijit/_WidgetsInTemplateMixin","epi/dependency","epi/shell/dnd/Target","epi/shell/command/_CommandProviderMixin","./_ContentAreaTree","./_ContentAreaTreeModel","./_TextWithActionLinksMixin","epi-cms/_ContentContextMixin","epi-cms/contentediting/ContentActionSupport","epi-cms/contentediting/viewmodel/ContentAreaViewModel","epi/shell/widget/ContextMenu","epi/shell/widget/_ValueRequiredMixin","epi-cms/widget/overlay/Block","epi-cms/widget/command/CreateContentFromSelector","epi-cms/widget/_HasChildDialogMixin","epi-cms/contentediting/command/BlockRemove","epi-cms/contentediting/command/BlockEdit","epi-cms/contentediting/command/MoveToPrevious","epi-cms/contentediting/command/MoveToNext","epi-cms/contentediting/command/MoveOutsideGroup","epi-cms/contentediting/command/Personalize","epi-cms/contentediting/command/SelectDisplayOption","dojo/text!./templates/ContentAreaEditor.html","epi/i18n!epi/cms/nls/episerver.cms.contentediting.editors.contentarea"],function(_1,_2,_3,_4,_5,on,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f,_10,_11,_12,_13,_14,_15,_16,_17,_18,_19,_1a,_1b,_1c,_1d,_1e,_1f,_20,_21,_22,_23){return _1([_9,_a,_c,_b,_17,_13,_f,_1a,_12],{baseClass:"epi-content-area-editor",res:_23,templateString:_22,value:null,multiple:true,parent:null,overlayItem:null,model:null,intermediateChanges:true,editMode:"onpageedit",_contextService:null,_preventOnBlur:false,_dndTarget:null,constructor:function(){this.allowedDndTypes=[];},onChange:function(_24){},_handleModelChange:function(_25){this.validate();this.onChange(_25);},_onBlur:function(){if(this._preventOnBlur){return;}this.inherited(arguments);},focus:function(){if(this.model.get("value").length>0){this._focusManager.focus(this.treeNode);}else{this.textWithLinks.focus();}},postMixInProperties:function(){this.inherited(arguments);this.commands=this._commands||[new _1c(),new _21(),new _20({category:null}),new _1f(),new _1d(),new _1e(),new _1b()];this.model=new _15();this.own(this.model);this.own(this.model.watch("selectedItem",_2.hitch(this,function(_26,_27,_28){this.updateCommandModel(_28);})));this.own(on(this.model,"changed",_2.hitch(this,function(){if(!this._started||this._supressValueChanged){return;}this._set("value",this.model.get("value"));this._handleModelChange(this.value);})));},postCreate:function(){this.inherited(arguments);this.set("emptyMessage",_23.emptymessage);if(this.parent&&this.overlayItem){this.connect(this.parent,"onStartEdit",function(){this._selectFromOverlay(this.overlayItem.model);});}this.own(_6.subscribe("/dnd/start",_2.hitch(this,this._startDrag)));},buildRendering:function(){this.inherited(arguments);this._contextService=this._contextService||_d.resolve("epi.shell.ContextService");this.contextMenu=new _16();this.contextMenu.addProvider(this);this.own(this.contextMenu);this.setupActionLinks(this.actionsContainer);this.tree=new _10({accept:this.allowedDndTypes,contextMenu:this.contextMenu,model:new _11({model:this.model})},this.treeNode);this.own(this.tree);this._dndTarget=new _e(this.actionsContainer,{accept:this.allowedDndTypes,isSource:false,alwaysCopy:false,insertNodes:function(){}});this.own(_3.after(this._dndTarget,"onDropData",_2.hitch(this,function(_29,_2a,_2b,_2c){var _2d=_29[0].data;this.model.modify(_2.hitch(this,function(){this.model.addChild(_2d);}));}),true));},startup:function(){this.inherited(arguments);this.tree.startup();this.contextMenu.startup();},isCreateLinkVisible:function(){var _2e=new _5();var _2f=this._contextService&&this._contextService.currentContext;_7(this.getCurrentContent(),function(_30){var _31=true;if(_2f){_31=_2f.capabilities.resourceable&&!(_2f.currentMode==="create"||_2f.currentMode==="translate");}var _32=_14.isActionAvailable(_30,_14.action.Create,_14.providerCapabilities.Create,true);_2e.resolve(!!(_31&&_32));});return _2e.promise;},executeAction:function(_33){if(_33==="createnewblock"){this._preventOnBlur=true;var _34=new _19({creatingTypeIdentifier:"episerver.core.blockdata",createAsLocalAsset:true,canChangeLocalAssetName:false});_34.set("model",{save:_2.hitch(this,function(_35){this._preventOnBlur=false;var _36=_2.clone(this.get("value"),true)||[];_36.push(_35);this.set("value",_36);this.onChange(_36);this.onBlur();}),cancel:_2.hitch(this,function(){this._preventOnBlur=false;})});_34.execute();}},isValid:function(){return (!this.required||this.model.get("value").length>0);},_setReadOnlyAttr:function(_37){this._set("readOnly",_37);_4.set(this.actionsContainer,"display",_37?"none":"");if(this._source){this._source.isSource=!this.readOnly;}if(this.model){this.model.set("readOnly",_37);}this.tree.set("readOnly",_37);},_checkAcceptance:function(_38,_39){return this.readOnly?false:this._source.defaultCheckAcceptance(_38,_39);},_setValueAttr:function(_3a){this._set("value",_3a||[]);this.tree.set("selectedItems",null);this._supressValueChanged=true;this.model.set("value",_3a);this._supressValueChanged=false;},_selectFromOverlay:function(_3b){var _3c=_3b&&_3b.selectedItem&&_3b.selectedItem.serialize(),_3d=this.model,_3e=["root"];if(!_3c){return;}if(_3c.contentGroup){_3d=_3d.getChild({name:_3c.contentGroup});_3e.push(_3d.id);}_3d=_3d.getChild(_3c);if(!_3d){return;}_3e.push(_3d.id);_3d.set("selected",true);_3d.set("ensurePersonalization",_3b.selectedItem.ensurePersonalization);this.tree.set("path",_3e);},_startDrag:function(_3f,_40,_41){var _42=_8.getEnclosingWidget(_40[0]);if(_42.isInstanceOf(_18)&&this.parent.cancel){this.parent.set("isModified",false);this.parent.cancel();}}});});