import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UserLogin } from "../types/user-types";
import { shareReplay } from "rxjs/operators"

@Injectable()
export class AuthService {

  constructor(private http: HttpClient) {
  }

  login(username: string, password: string) {
    var request = this.http.post<UserLogin>('/Api/User-Management/Login', { username, password }, { observe: "response" })
      .pipe(shareReplay(1));

    request.subscribe(s => {
      this.setSession(s.body!);
    });

    return request;
  }

  ensureNotOutdated(): any {
    const expiration = localStorage.getItem('expires_at');
    if (expiration == null) {
      return false;
    }
    const loggedIn = Date.now() < Number.parseInt(expiration);
    if (!loggedIn) {
      this.logout();
    }
  }

  logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_id');
    localStorage.removeItem('nick_name');
    localStorage.removeItem('user_name');
    localStorage.removeItem('expires_at');
  }

  isLoggedIn() {
    this.ensureNotOutdated();

    const expiration = localStorage.getItem('expires_at');
    return expiration != null;
  }

  isLoggedOut() {
    return !this.isLoggedIn();
  }

  getToken() {
    this.ensureNotOutdated();
    return localStorage.getItem('jwt_token');
  }
  getUserId() {
    this.ensureNotOutdated();
    const raw = localStorage.getItem('user_id');
    if (raw == null) {
      return null;
    }
    const userId = Number.parseInt(raw);

    return userId;
  }
  setNickname(nickname: string | null) {
    if (nickname == null) {
      localStorage.removeItem('nick_name');
    } else {
      localStorage.setItem('nick_name', nickname);
    }
  }
  getDisplayName() {
    const nickname: string | null = this.getNickname();
    return nickname == null || nickname.length == 0
      ? this.getUsername()
      : nickname;
  }
  getNickname() {
    return localStorage.getItem('nick_name');
  }
  setUsername(username: string) {
    localStorage.setItem('user_name', username);
  }
  getUsername() {
    this.ensureNotOutdated();
    return localStorage.getItem('user_name');
  }

  private setSession(login: UserLogin) {
    const expiresAt = Date.now() + login.expiresIn * 1000;

    localStorage.setItem('jwt_token', login.token);
    localStorage.setItem('user_id', login.userSnippet.id.toString());
    localStorage.setItem('user_name', login.userSnippet.username);
    localStorage.setItem('nick_name', login.userSnippet.nickname);
    localStorage.setItem('expires_at', expiresAt.toString());
  }
}
