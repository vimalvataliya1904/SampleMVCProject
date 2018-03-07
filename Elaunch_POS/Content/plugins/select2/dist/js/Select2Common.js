var settings;
var self;
(function ($) {
    $.fn.createAutocomplete = function (options) {
        var defaults = {
            Url: '/Home/AutoCompleteData',
            Mode: '',
            SelectedText: '',
            SelectedValue: '',
            PlaceHolder:''
        };
        self = this;
        settings = $.extend({}, defaults, options);
        $(this).select2({
            ajax: {
                url: settings.Url,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        q: params.term,
                        mode: $(this).attr('mode'),
                        relatedTo:$(this).attr('relatedTo')?$(this).attr('relatedTo'):''
                };
    },
    processResults: function (data, params) {
        params.page = params.page || 1;
        return {
            results: data
        };
    },
    cache: true
},
escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
minimumInputLength: 0,
    placeholder: settings.PlaceHolder,
allowClear: true,
//templateSelection: function (data, container) {
//    return data.text;
//},
//templateResult: function (data) {
//    return "<option>" + data.text + "</option>";
//},
theme: "classic"
});

if (settings.SelectedText != '' && settings.SelectedValue != '') {
    $(this).empty().append($("<option/>").val(settings.SelectedValue).text(settings.SelectedText)).val(settings.SelectedValue).trigger("change");
}
}
}(jQuery));