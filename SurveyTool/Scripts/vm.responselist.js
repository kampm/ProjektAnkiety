var ResponseListModel = function() {
    var self = this;

    self.destroy = function (item, event) {
        var $target = $(event.target);

        if (confirm('Na pewno chcesz usunąć?')) {
            $.post($target.attr('href'), function() {
                $target.parents('tr').remove();
            });
        }

        return false;
    };
};