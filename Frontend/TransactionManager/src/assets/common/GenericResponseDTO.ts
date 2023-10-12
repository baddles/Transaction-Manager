export type GenericResponseDTO<T> = {
    httpStatusCode: number,
    message: string,
    data: T,
    timestamp: string
  }