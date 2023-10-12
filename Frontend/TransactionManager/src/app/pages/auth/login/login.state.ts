export type LoginStatus = null | "" | "Loading" | "Success" | "Failed"
export interface LoginState {
    handle(): LoginStatus;
}
export class LoginInitialState implements LoginState {
    constructor(context: any, changeSize: Function) {
        changeSize.call(context)
    }
    handle(): LoginStatus {
        return "";
    }
}

export class LoginLoadingState implements LoginState {
    constructor(context: any, changeSize: Function) {
        changeSize.call(context)
    }
    handle(): LoginStatus {
        return "Loading";
    }
}


export class LoginSuccessState implements LoginState {
    constructor(context: any, changeSize: Function) {
        changeSize.call(context)
    }
    handle(): LoginStatus {
        return "Success";
    }
}


export class LoginFailedState implements LoginState {
    constructor(context: any, changeSize: Function) {
        changeSize.call(context)
    }
    handle(): LoginStatus {
        return "Failed";
    }
}