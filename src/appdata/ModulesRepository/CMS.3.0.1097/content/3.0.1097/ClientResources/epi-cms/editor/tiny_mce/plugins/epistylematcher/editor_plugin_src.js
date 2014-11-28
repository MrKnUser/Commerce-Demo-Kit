(function(tinymce){tinymce.create("tinymce.plugins.epistylematcher",{init:function(ed){ed.onInit.add(function(ed){var formatter=ed.formatter,oldMatchAll=formatter.matchAll,oldRemove=formatter.remove;formatter.matchAll=function(){var list=oldMatchAll.apply(this,arguments);return list=list.sort(function(item1,item2){var format1=formatter.get(item1)[0],format2=formatter.get(item2)[0];if(format1.block==format2.block&&format1.inline==format2.inline){if(!format1.classes)return 1;if(!format2.classes)return-1;var classes1=format1.classes.length>format2.classes.length?format1.classes:format2.classes,classes2=format1.classes.length>format2.classes.length?format2.classes:format1.classes,contains=!0;for(var i in classes2){var found=!1;for(var j in classes1)if(classes1[j]==classes2[i]){found=!0;break}if(!found){contains=!1;break}}return contains?format1.classes.length>format2.classes.length?-1:1:0}return 0})},formatter.remove=function(name,vars,node){if(name)oldRemove.apply(this,arguments);else{var currentNode,sel=ed.selection;if((currentNode=sel.getStart())==sel.getEnd()){var currentNodeFormat=ed.dom.isBlock(currentNode)?{block:currentNode.tagName.toLowerCase()}:{inline:currentNode.tagName.toLowerCase()};this.register("__current__",currentNodeFormat),oldRemove.apply(this,["__current__",vars,node])}}}})},getInfo:function(){return{longname:"styling improvement plugin",author:"EPiServer AB",authorurl:"http://www.episerver.com",infourl:"http://www.episerver.com",version:1}}}),tinymce.PluginManager.add("epistylematcher",tinymce.plugins.epistylematcher)})(tinymce,epiJQuery);