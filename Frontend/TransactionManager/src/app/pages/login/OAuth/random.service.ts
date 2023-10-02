import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import API_KEY from '../../../../assets/API/API_KEY.json'
import API_PATH from '../../../../assets/API/API_PATH.json'
import CONFIG from '../../../../assets/configs/config.json'
@Injectable({
  providedIn: 'root'
})
export class RandomService {

  constructor(private httpService: HttpClient) { }
  invoke(id: string) {
    return this.httpService.post(API_PATH['random.org'].invoke, {
      'jsonrpc': CONFIG['random.org'].jsonrpg,
      'method': CONFIG['random.org'].method.SignedAPI.generateSignedStrings,
      'params': {
        'apiKey': API_KEY['random.org'].apiKey,
        'n': CONFIG['random.org'].n,
        'length': CONFIG['random.org'].length,
        'characters': CONFIG['random.org'].characters
      },
      'id': id
    })
  }

}
