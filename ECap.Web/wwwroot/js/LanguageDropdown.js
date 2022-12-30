var app = new Vue({
    el: '#LanguageSection',
    data: {
        languages: [{
            "id": "en-US",
            "Name":"English",
        },
        {
            "id":"ko-KR",
            "Name": "Korean",
        },
        {
            "id":"ja-JP",

            "Name": "Japan",
        }]
    },

    methods: {
        selectedvalue: function () {
            var culture = $("#ddlLanguage option:selected").val();
            $.post("/profile/SetLanguage?culture=" + culture, {})
                .done(function (data) {
                    if (data) {
                        window.location.href = "/profile/Login";
                    }
                    else {
                        if (data.message) {
                            self.message = data.message;
                        }
                    }
                 });
            }
        }
    })

