var app = new Vue({
    el: "#ChangePwdApp",
    data: {
        password: "",
        confirmPassword: "",
        modelState: {},
        message: ""
    },
    methods: {
        clearData: function () {
            var vm = this;
            vm.password = "";
            vm.confirmPassword = "";
        },
        showPasswordConfirm: function () {
            var e = document.getElementById("password-input-confirm");
            "password" === e.type ? (e.type = "text") : (e.type = "password");
        },
        showPassword: function () {
            var e = document.getElementById("password-input");
            "password" === e.type ? (e.type = "text") : (e.type = "password");
        },
        blurEventHandler: function () {
            var self = this;
            self.modelState = {};
            self.message = "";
        },
        submitRequest: function () {
            
            var self = this;
            //Clear the modelState before submission
            self.modelState = {};
            self.message = "";

            //validate password
            if (self.password === '') {
                self.$set(self.modelState, "Password", 'Password is required.');
                return;
            }

            //validate confirmPassword
            if (self.confirmPassword === '') {
                self.$set(self.modelState, "ConfirmPassword", 'Confirm Password is required.');
                return;
            }

            if (self.password != self.confirmPassword) {
                self.$set(self.modelState, "ConfirmPassword", "'ConfirmPassword' and 'Password' do not match.");
                return;
            }

            var newModel = {
                password: self.password,
                confirmPassword: self.confirmPassword,
            }

            $.post("/Admin/ChangePassword", newModel)
                .done(function (data) {
                    if (data.status) {
                        self.message = data.message;
                        self.clearData();
                    } else {
                        if (data.message) {
                            self.message = data.message;
                        }
                        else {
                            self.modelState = {};
                            if (data.modalStateError.length > 0) {
                                $.each(JSON.parse(data.modalStateError), function (index, item) {
                                    let message = '';
                                    let stateErrors = item.Value;
                                    if (stateErrors) {
                                        $.each(stateErrors, function (j, stateError) {
                                            if (j > 0) {
                                                message += "; ";
                                            }
                                            message += stateError;
                                        });
                                        self.$set(self.modelState, item.Key, message);
                                    }
                                });
                            }
                        }
                    }
                }).fail(function () {
                    // .error("Can not update this bug.");
                }).always(function () {
                    //self.clearData();
                });
        }
    }
});