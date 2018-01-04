(function ($) {
    $.fn.SharkDevTreeView = function (options) {

        var optionsObj = new function (ctrlId) {
            this.AfterSelectHandler = options.AfterSelectHandler;
            this.DataOnClient = options.DataOnClient;
            this.AutoCompleteHandler = options.AutoCompleteHandler;
            this.ControlId = ctrlId;
        }(this.attr("id"));

        var rootUl = optionsObj.ControlId + "_rootUl";
        var strHiddenUl = 'hiddenUl';
        var firstLhATag = 'Expander_0';

        ProgressBar = function (parentId) {
            var i = 1;
            var base = "progressCell";
            var stop = false;
            var progressParent = $('#' + parentId + ' #progressParent')[0];
            var progressChilds = $('#' + parentId + ' #progressParent > span');
            var intervalId;

            this.Start = function () {
                $(progressParent).css('display', 'inline');
                var that = this;
                intervalId = setInterval(function () { that.Loop(); }, 300);
            }

            this.Loop = function () {
                if (i > 3) {
                    this.ResetControls();
                    i = 1;
                }
                else {
                    this.ResetControls();
                    $(progressChilds).each(function (e, elem) {
                        var id = base + i;
                        if (elem.id == id) {
                            $(elem).css('padding-top', '2px');
                            $(elem).css('padding-bottom', '2px');
                            $(elem).css('padding-left', '2px');
                            return false;
                        }
                    });
                    ++i;
                }
            }

            this.Stop = function () {
                $(progressParent).css('display', '');
                i = 1;
                stop = false;
                this.ResetControls();
                clearInterval(intervalId);
            }

            this.ResetControls = function () {
                $(progressChilds).css('padding-top', '0px');
                $(progressChilds).css('padding-bottom', '0px');
                $(progressChilds).css('padding-left', '0px');
            }
        }
        
        ScrollToElement = function (elementid, optionsParam) {
            $('#' + optionsParam.ControlId + '_rootUl').removeClass('hiddenUl');
            var arrElemsToExpand = $('#' + optionsParam.ControlId + ' #Content_' + elementid).parents('li');
            for(var i=0; i < arrElemsToExpand.length; i++){
                $(arrElemsToExpand[i]).removeClass(strHiddenUl).removeClass('contentSelected');
            }
            var lastElem = arrElemsToExpand[0];
            for (var i = 0; ; i++) {
                lastElem = $(lastElem).parent().closest('li').get(0);
                var e = $(lastElem).find('a');
                if (e.length > 0) {
                    $(e[0]).removeClass('expand');
                    $(e[0]).addClass('collapse');
                }
                if (lastElem == undefined) break;
            }            

            var headerDiv = optionsParam.ControlId + "_header";
            var verticalOffSet = $('#' + headerDiv).offset().top + 20;
            var divAroundUl = optionsParam.ControlId + "_tree";
                
            $("#" + divAroundUl).animate({
                scrollTop: 0,
                scrollLeft: 0
            }, 0, function () {
                if ($('#' + optionsParam.ControlId + ' #Content_' + elementid).get(0) != undefined) {
                        
                    var top = $('#' + optionsParam.ControlId + ' #Content_' + elementid).offset().top;
                    var left = $('#' + optionsParam.ControlId + ' #Content_' + elementid).offset().left;
                    var right = $('#' + optionsParam.ControlId + ' #Content_' + elementid).offset().right;

                    $("#" + divAroundUl).animate({
                        scrollTop: top - verticalOffSet,
                        scrollRight: right
                    }, 200, function () {
                        SelectElement($('#' + optionsParam.ControlId + ' #Content_' + elementid), optionsParam);
                    });
                }
            });
        }

        SelectElement = function (elementid, optionsParam) {            
            if ($('#' + optionsParam.ControlId + ' a[id="Expander_0"]').hasClass('expand')) {
                $('#' + optionsParam.ControlId + ' a[id="Expander_0"]').removeClass('expand');
                $('#' + optionsParam.ControlId + ' a[id="Expander_0"]').addClass('collapse');
            }

            var retParam = $(elementid).attr('obj');
            $('#' + optionsParam.ControlId + ' span[class=contentSelected]').parent().removeClass('hover2');
            $('#' + optionsParam.ControlId + ' span[class=contentSelected]').removeClass('contentSelected');
            var innerText = $(elementid).text();
            $(elementid).text('');
            $(elementid).addClass('hover2');
            $(elementid).append('<span id="contentInside" class="contentSelected">' + innerText + '</span>');
            var element = $(elementid).prev('a[id^="Expander"]');
            if (optionsParam.AfterSelectHandler != null) {
                optionsParam.AfterSelectHandler(JSON.parse(retParam));
            }
        }

        BindMethods = function () {            
            return function (containerControlId, optionsParam) {
                var progressBar;
                $("#" + optionsParam.ControlId + "_autoCompleteInput").autocomplete({
                    source: function (request, response) {
                        if (optionsParam.DataOnClient) {
                            var obj = JSON.parse($('#' + optionsParam.ControlId + '_JsonData').text());
                            response($.map(obj, function (item) {
                                if (item.Term.toLowerCase().indexOf(request.term.toLowerCase()) == 0) {
                                    return {
                                        label: item.Term,
                                        id: item.Id
                                    }
                                }
                            }));
                        }
                        else {
                            var p = (g.AutoCompleteHandler.indexOf("?") > 0) ? "&q=" : "?q=";
                            $.ajax({
                                type: "POST",
                                url : g.AutoCompleteHandler + p + l.term,
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    var obj = data;
                                    response($.map(obj, function (item) {
                                        return {
                                            label: item.Term,
                                            id: item.Id
                                        }
                                    }));
                                }
                            });
                        }
                    },
                    appendTo: $("#" + optionsParam.ControlId + "_autoCompleteInput").parent(),
                    minLength: 2,
                    messages: {
                        noResults: '',
                        results: function (event, ui) { }
                    },
                    response: function (event, ui) {
                        progressBar.Stop();
                        progressBar = null;
                    },
                    search: function (event, ui) {
                        progressBar = new ProgressBar(optionsParam.ControlId);
                        progressBar.Start();
                    },
                    select: function (event, ui) {
                        ScrollToElement(ui.item.id, optionsParam);
                    }
                });

                var contentArr = $('#' + containerControlId + ' span[id^="Content"]');
                $(contentArr).mouseover(function () { $(this).addClass('hover'); });
                $(contentArr).mouseleave(function () { $(this).removeClass('hover'); });
                $(contentArr).click(function () { SelectElement(this, optionsParam); });

                $('#' + containerControlId + ' a[id^="Expander"]').click(function (e) {
                    var li = $(this).parents('li').get(0);

                    if ($(li).hasClass(strHiddenUl) == true) {
                        $(li).removeClass(strHiddenUl);
                    }
                    else {
                        $(li).addClass(strHiddenUl);
                    }
                    
                    if ($(this).hasClass('expand')) {
                        $(this).removeClass('collapse');
                        $(this).removeClass('expand');

                        $(this).addClass("collapse");
                    }
                    else {
                        $(this).removeClass('collapse');
                        $(this).removeClass('expand');

                        $(this).attr('class', '');
                        $(this).addClass("expand");
                    }
                });
                
                $('#' + optionsParam.ControlId + ' a[id="Expander_0"]').off('click');
                $('#' + optionsParam.ControlId + ' a[id^="Expander_0"]').click(function (e) {
                    if ($('#' + rootUl).hasClass(strHiddenUl) == true) {
                        $('#' + rootUl).removeClass(strHiddenUl);

                        $(this).attr('class', '');
                        $(this).addClass('collapse');
                    }
                    else {
                        $(this).attr('class', '');
                        $(this).addClass('expand');

                        $('#' + rootUl).addClass(strHiddenUl);
                    }
                });
            }(rootUl, optionsObj);
        }();
    };
})(jQuery);