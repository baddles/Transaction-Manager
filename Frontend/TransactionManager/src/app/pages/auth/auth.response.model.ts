export type initializeResponseData = {
    client_id: string
    public_key: string
}
export type loginResponseData = {
    access_token: string,
    token_type: string,
    refresh_token: string,
    expires_at: string
  }