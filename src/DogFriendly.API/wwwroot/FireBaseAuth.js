$(document).ready(function () {
    function setNativeValue(element, value) {
        const valueSetter = Object.getOwnPropertyDescriptor(element, 'value').set;
        const prototype = Object.getPrototypeOf(element);
        const prototypeValueSetter = Object.getOwnPropertyDescriptor(prototype, 'value').set;

        if (valueSetter && valueSetter !== prototypeValueSetter) {
            prototypeValueSetter.call(element, value);
        } else {
            valueSetter.call(element, value);
        }
    }

    function setJwt(key) {
        var inputAuth = $('.auth-container :input[type=text]')[0];
        setNativeValue(inputAuth, 'Bearer ' + key);
        inputAuth.dispatchEvent(new Event('input', { bubbles: true }));
    }

    setTimeout(function () {
        var token = null;
        $.get("/api/firebase", function (config) {
            var config = {
                apiKey: config.apiKey,
                authDomain: config.authDomain,
                projectId: config.projectId,
                appId: config.appId
            };
            firebase.initializeApp(config);
            firebase.auth().onAuthStateChanged(function (user) {
                if (user) {
                    $('#display-name').text(user.displayName);
                    user.getIdToken().then(function (accessToken) {
                        token = accessToken;
                        $('#sign-out').show();
                        $('#firebaseui-auth-container').hide();
                    });
                } else {
                    $('#sign-out').hide();
                    $('#firebaseui-auth-container').show();
                }
            }, function (error) {
                console.error(error);
            });

            var uiConfig = {
                signInFlow: 'popup',
                signInSuccessUrl: window.location.href,
                signInOptions: [
                    firebase.auth.GoogleAuthProvider.PROVIDER_ID,
                    firebase.auth.EmailAuthProvider.PROVIDER_ID,
                    {
                        provider: 'microsoft.com',
                        providerName: 'Microsoft',
                        fullLabel: 'Login with Microsoft',
                        buttonColor: '#2F2F2F',
                        iconUrl: 'microsoft.jpeg',
                        loginHintKey: 'login_hint'
                    },
                    firebase.auth.FacebookAuthProvider.PROVIDER_ID,
                ],
                tosUrl: window.location.href,
                privacyPolicyUrl: window.location.href
            };
            var ui = new firebaseui.auth.AuthUI(firebase.auth());
            $('<div id="firebaseui-auth-container" style="width:720px"></div><button id="sign-out" class="btn authorize locked"><span id="display-name"></span><span>Sign-out</span><svg width="20" height="20"><use href="#locked" xlink:href="#locked"></use></svg></button>').insertBefore('button.authorize');
            ui.start('#firebaseui-auth-container', uiConfig);
            $('#sign-out').click(function () {
                signOut();
            });
            $('.btn.authorize.unlocked').click(function () {
                if (token != null) {
                    setTimeout(function () {
                        setJwt(token);
                    }, 2000);
                }
            });
        });

        function signOut() {
            firebase.auth().signOut();
            token = null;
        }

        setInterval(() => {
            if (token !== null && firebase.auth().currentUser !== null) {
                firebase.auth().currentUser.getIdToken(true)
                    .then(() => {})
                    .catch(() => {
                        signOut();
                    });
            }
        }, 5000);
    }, 2000);
});
