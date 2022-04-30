import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { shareReplay } from "rxjs/operators";
import { AccountInfo } from "../types/account-types";
import { CallbackInfo } from "../types/callback-types";
import { PaginatedResponse } from "../types/general-types";
import { PostSnippet } from "../types/post-types";
import { ProfileInfo } from "../types/profile-types";
import { ShareData, ShareInfo, ShareSecrets, ShareSnippet, ShareUserData } from "../types/share-types";
import { SubscriptionInfo, SubscriptionSnippet, SubscriptionType } from "../types/subscription-types";
import { UserSnippet } from "../types/user-types";
import { AuthService } from "./authservice";

@Injectable()
export class WeShareClient {
  constructor(private client: HttpClient, private authService: AuthService) { }

  getHeaders() {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: "Bearer " + this.authService.getToken()
    });
  }
  boolToStr(bool: boolean | null) {
    return bool ? "true" : "false";
  }

  //User

  createUser(username: string, email: string, password: string) {
    return this.client.post("Api/User/Create", { username, email, password })
      .pipe(shareReplay(1));
  }

  getUserSnippet(username: string) {
    return this.client.get<UserSnippet>("Api/User/Snippet/Name/" + username)
      .pipe(shareReplay(1));
  }

  //Profile

  updateProfile(nickname: string | null, likesPublished: boolean | null) {
    const userId = this.authService.getUserId();
    return this.client.post("Api/Profile/" + userId + "/Update/", { nickname, likesPublished: this.boolToStr(likesPublished) }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getProfileInfoByUsername(username: string) {
    return this.client.get<ProfileInfo>("Api/Profile/Name/" + username)
      .pipe(shareReplay(1));
  }

  getProfileInfoById(userId: number) {
    return this.client.get<ProfileInfo>("Api/Profile/Id/" + userId)
      .pipe(shareReplay(1));
  }

  //Account

  getAccountInfo(userId: number) {
    return this.client.get<AccountInfo>("Api/Account/Info/Id/" + userId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  updateAccount(username: string | null, email: string | null) {
    const userId = this.authService.getUserId();
    return this.client.post("Api/Account/" + userId + "/Update", { username, email }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  requestPasswordReset(email: string) {
    return this.client.post("Api/Account/RequestPasswordReset", { email })
      .pipe(shareReplay(1));
  }

  //Shares

  createShare(name: string, description: string, readme: string) {
    const userId = this.authService.getUserId();
    return this.client.post<number>("Api/Share/" + userId + "/Create", { name, description, readme }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  updateShare(shareId: number, name: string | null, description: string | null, readme: string | null) {
    return this.client.post("Api/Share/Update/" + shareId, { name, description, readme }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  deleteShare(shareId: number) {
    return this.client.delete("Api/Share/Delete/" + shareId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareInfo(shareId: number) {
    return this.client.get<ShareData>("Api/Share/Data/Id/" + shareId)
      .pipe(shareReplay(1));
  }

  getShareUserData(shareId: number) {
    const userId = this.authService.getUserId();
    return this.client.get<ShareUserData>("Api/Share/UserData/" + userId + "/Id/" + shareId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareSnippets(username: string, page: number) {
    return this.client.get<PaginatedResponse<ShareSnippet>>("Api/User/Shares/Name/" + username + "/" + page + "/" + 10)
      .pipe(shareReplay(1));
  }

  getPopularShareSnippets(username: string) {
    return this.client.get<ShareSnippet[]>("Api/User/Shares/Name/" + username + "/" + 0 + "/" + 4 + "?ordering=SubscriberCount")
      .pipe(shareReplay(1));
  }

  getLikedShareSnippets(username: string, page: number) {
    return this.client.get<PaginatedResponse<ShareSnippet>>("Api/User/Shares/Liked/Name/" + username + "/" + page + "/" + 10, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareSecrets(shareId: number) {
    return this.client.get<ShareSecrets>("Api/Share/Secrets/Id/" + shareId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  //Subscriptions

  getSubscriptionInfo(subscriptionId: number) {
    return this.client.get<SubscriptionInfo>("Api/Subscription/Info/Id/" + subscriptionId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getSubscriptionSnippets(userId: number, type: SubscriptionType | null, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<SubscriptionSnippet>>("Api/User/Subscription/Snippets/Id/" + userId + "/" + page + "/" + pageSize + (type != null ? "?type=" + type : ""),
      { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareSubscriptionSnippets(userId: number, shareId: number, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<SubscriptionSnippet>>("Api/User/Share/Subscription/Snippets/Id/" + userId + "/" + shareId + "/" + page + "/" + pageSize, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getUnsentPosts(subscriptionId: number) {
    return this.client.get<PaginatedResponse<PostSnippet>>("Api/Subscription/Posts/Unsent/Id/" + subscriptionId + "/" + 0 + "/" + 10, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  createDashboardSubscription(shareId: number, name: string) {
    return this.createSubscription(shareId, SubscriptionType.Dashboard, name, null);
  }
  createWebhookSubscription(shareId: number, name: string, targetUrl: string) {
    return this.createSubscription(shareId, SubscriptionType.Webhook, name, targetUrl);
  }

  private createSubscription(shareId: number, type: SubscriptionType, name: string, targetUrl: string | null) {
    const userId = this.authService.getUserId();
    return this.client.post<number>("Api/Subscription/Create/", { shareId, userId, type, name, targetUrl }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  markPostAsSent(subscriptionId: number, postId: number) {
    return this.client.post("Api/Subscription/" + subscriptionId + "/MarkAsSent/" + postId, {}, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  removeSubscription(subscriptionId: number) {
    return this.client.delete("Api/Subscription/Remove/" + subscriptionId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  //Posts

  getPosts(shareId: number, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<PostSnippet>>("Api/Share/Posts/Metadata/" + shareId + "/" + page + "/" + pageSize)
      .pipe(shareReplay(1));
  }

  //Likes

  addLike(shareId: number) {
    const userId = this.authService.getUserId();
    return this.client.post("Api/Like/Add/" + shareId + "/" + userId, {}, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  removeLike(shareId: number) {
    const userId = this.authService.getUserId();
    return this.client.post("Api/Like/Remove/" + shareId + "/" + userId, {}, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  // Callback

  getCallbackInfo(callbackSecret: string) {
    return this.client.get<CallbackInfo>("Api/Callback/Info/Secret/" + callbackSecret)
      .pipe(shareReplay(1));
  }

  handleVerifyEmailCallback(callbackSecret: string) {
    return this.client.post("Api/Callback/Handle/VerifyEmail", { callbackSecret })
      .pipe(shareReplay(1));
  }

  resetPassword(callbackSecret: string, password: string) {
    return this.client.post("Api/Callback/Handle/PasswordReset", { callbackSecret, password })
      .pipe(shareReplay(1));
  }
}
