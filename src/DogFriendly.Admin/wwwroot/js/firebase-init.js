var fb;

window.loginApplication = function (token) {
    const formData = new FormData();
    formData.append('token', token);
    $.ajax({
        url: '/api/authentication/login',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false
    });
};

window.getFirebaseAuthToken = async function () {
    if (fb == null)
        fb = firebase.initializeApp(firebaseConfig);

    const user = fb.auth().currentUser;
    if (user) {
        return await user.getIdToken();
    }

    return null;
};

window.isFirebaseUserAuth = async function () {
    return (await window.getFirebaseAuthToken()) !== null;
}

window.updateFirebaseAuth = async function (dotNetHelper) {
    if (fb == null) {
        fb = firebase.initializeApp(firebaseConfig);
        fb.auth().onAuthStateChanged(async function (user) {
            if (user == null) {
                $.post('/api/authentication/logout');
                await dotNetHelper.invokeMethodAsync('SignoutUser');
            }
            else if (!user.emailVerified) {
                await user.sendEmailVerification();
            }
            else {
                const token = await user.getIdToken();
                window.loginApplication(token);
                await dotNetHelper.invokeMethodAsync('SigninUser', token);
            }
        }, function (error) {
            console.error(error);
        });
    }
};

window.initFirebaseUi = async function (dotNetHelper) {
    await window.updateFirebaseAuth(dotNetHelper);
    if ($('#firebaseui-auth-container').length > 0
        && !(await window.isFirebaseUserAuth())) {
        try {
            var uiConfig = {
                signInFlow: 'popup',
                signInSuccessUrl: window.location.href,
                signInOptions: [
                    firebase.auth.EmailAuthProvider.PROVIDER_ID,
                    firebase.auth.GoogleAuthProvider.PROVIDER_ID,
                    {
                        provider: 'microsoft.com',
                        providerName: 'Microsoft',
                        fullLabel: 'Se connecter avec Microsoft',
                        buttonColor: '#2F2F2F',
                        iconUrl: 'img/microsoft.jpeg',
                        loginHintKey: 'login_hint'
                    },
                    firebase.auth.FacebookAuthProvider.PROVIDER_ID,
                ],
                tosUrl: window.location.href,
                privacyPolicyUrl: window.location.href,
                callbacks: {
                    signInSuccessWithAuthResult: function (authResult) {
                        window.loginFirebaseAuth(authResult, dotNetHelper);
                        return false;
                    }
                }
            };
            new firebaseui.auth.AuthUI(fb.auth()).start('#firebaseui-auth-container', uiConfig);
        } catch { }
    }
}

window.loginFirebaseAuth = async function (authResult, dotNetHelper) {
    const user = authResult.user;
    const token = await user.getIdToken();
    window.loginApplication(token);
    await dotNetHelper.invokeMethodAsync('LoginUser', token);
}

window.logoutFirebaseAuth = async function () {
    if (fb != null) {
        fb.auth().signOut();
    }
}