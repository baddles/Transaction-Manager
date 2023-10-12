import { LoginInitialState, LoginState } from "./login.state";

export class LoginContext {
    private state: LoginState

    constructor(initialState: LoginState = new LoginInitialState(null, function() {})) {
        this.state = initialState;
    }

    setState(state: LoginState): void {
        this.state = state;
    }

    request(): string {
        let result = this.state.handle();
        if (result === null) {
            console.error("result is null");
            return "";
        }
        return result;
    }
}