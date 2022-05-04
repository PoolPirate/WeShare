import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map, shareReplay } from "rxjs/operators";
import { AccountInfo, ServiceConnectionSnippet, ServiceConnectionType } from "../types/account-types";
import { CallbackInfo } from "../types/callback-types";
import { PaginatedResponse } from "../types/general-types";
import { ParsedPostContent, PostContent, PostSendInfo, PostSnippet } from "../types/post-types";
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
    return this.client.post("Api/Users", { username, email, password })
      .pipe(shareReplay(1));
  }

  getUserSnippet(username: string) {
    return this.client.get<UserSnippet>("Api/Users/ByUsername/" + username + "/Snippet")
      .pipe(shareReplay(1));
  }

  //Profile

  updateProfile(nickname: string | null, likesPublished: boolean | null) {
    const userId = this.authService.getUserId();
    return this.client.patch("Api/Profiles/" + userId, { nickname, likesPublished: this.boolToStr(likesPublished) }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getProfileInfoByUsername(username: string) {
    return this.client.get<ProfileInfo>("Api/Profiles/ByUsername/" + username + "/Info")
      .pipe(shareReplay(1));
  }

  getProfileInfoById(userId: number) {
    return this.client.get<ProfileInfo>("Api/Profiles/" + userId + "/Info")
      .pipe(shareReplay(1));
  }

  //Account

  getAccountInfo(userId: number) {
    return this.client.get<AccountInfo>("Api/Accounts/" + userId + "/Info", { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  updateAccount(username: string | null, email: string | null) {
    const userId = this.authService.getUserId();
    return this.client.patch("Api/Accounts/" + userId, { username, email }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  requestPasswordReset(email: string) {
    return this.client.post("Api/Account-Management/RequestPasswordReset", { email })
      .pipe(shareReplay(1));
  }

  createServiceConnection(type: ServiceConnectionType, code: string) {
    const userId = this.authService.getUserId();
    return this.client.post("Api/Accounts/" + userId + "/ServiceConnections", { type, code }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  removeServiceConnection(serviceConnectionId: number) {
    const userId = this.authService.getUserId();
    return this.client.delete("Api/Accounts/" + userId + "/ServiceConnections/" + serviceConnectionId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getServiceConnections(page: number, pageSize: number) {
    const userId = this.authService.getUserId();
    return this.client.get<PaginatedResponse<ServiceConnectionSnippet>>("Api/Accounts/" + userId + "/ServiceConnection-Snippets",
      { headers: this.getHeaders(), params: { "page": page, "pageSize": pageSize } })
      .pipe(shareReplay(1));
  }

  //Shares

  createShare(name: string, description: string, readme: string, isPrivate: boolean) {
    const userId = this.authService.getUserId();
    return this.client.post<number>("Api/Users/" + userId + "/Shares", { name, description, readme, isPrivate }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  updateShare(shareId: number, name: string | null, description: string | null, readme: string | null) {
    return this.client.patch("Api/Shares/" + shareId, { name, description, readme }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  updateShareVisibility(shareId: number, isPrivate: boolean) {
    return this.client.patch("Api/Shares/" + shareId + "/Visibility/", { isPrivate }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  deleteShare(shareId: number) {
    return this.client.delete("Api/Shares/" + shareId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareSnippet(shareId: number) {
    return this.client.get<ShareSnippet>("Api/Shares/" + shareId + "/Snippet", { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareInfo(shareId: number) {
    return this.client.get<ShareData>("Api/Shares/" + shareId + "/Data", { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareUserData(shareId: number) {
    const userId = this.authService.getUserId();
    return this.client.get<ShareUserData>("Api/Shares/" + shareId + "/UserData/" + userId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareSnippets(username: string, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<ShareSnippet>>("Api/Users/ByUsername/" + username + "/Share-Snippets",
      { params: { page: page, pageSize: pageSize }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getPopularShareSnippets(username: string, page: number, pageSize: number) {
    return this.client.get<ShareSnippet[]>("Api/Users/ByUsername/" + username + "/Share-Snippets",
      { params: { page: page, pageSize: pageSize, ordering: "SubscriberCount" }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getLikedShareSnippets(username: string, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<ShareSnippet>>("Api/Users/ByUsername/" + username + "/Liked-Share-Snippets",
      { params: { page: page, pageSize: pageSize }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareSecrets(shareId: number) {
    return this.client.get<ShareSecrets>("Api/Shares/" + shareId + "/Secrets", { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  //Subscriptions

  getSubscriptionInfo(subscriptionId: number) {
    return this.client.get<SubscriptionInfo>("Api/Subscriptions/" + subscriptionId + "/Info", { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getSubscriptionSnippets(userId: number, type: SubscriptionType | null, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<SubscriptionSnippet>>("Api/Users/" + userId + "/Subscription-Snippets",
      { params: { page: page, pageSize: pageSize, type: (type == null ? "" : type) }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getShareSubscriptionSnippets(userId: number, shareId: number, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<SubscriptionSnippet>>("Api/Users/" + userId + "/Shares/" + shareId + "/Subscription-Snippets",
      { params: { page: page, pageSize: pageSize }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getUnsentPosts(subscriptionId: number, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<PostSnippet>>("Api/Subscriptions/" + subscriptionId + "/Posts/Unsent",
      { params: { page: page, pageSize: pageSize }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }
  getPendingPosts(subscriptionId: number, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<PostSendInfo>>("Api/Subscriptions/" + subscriptionId + "/Posts/Pending",
      { params: { page: page, pageSize: pageSize }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }
  getReceivedPosts(subscriptionId: number, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<PostSendInfo>>("Api/Subscriptions/" + subscriptionId + "/Posts/Received",
      { params: { page: page, pageSize: pageSize }, headers: this.getHeaders() })
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
    return this.client.post<number>("Api/Subscriptions", { shareId, userId, type, name, targetUrl }, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  markPostAsSent(subscriptionId: number, postId: number) {
    return this.client.post("Api/Subscriptions/" + subscriptionId + "/Posts/" + postId + "/Receive", {}, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  removeSubscription(subscriptionId: number) {
    return this.client.delete("Api/Subscriptions/" + subscriptionId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  //Posts

  submitPost(shareSecret: string, postHeaders: [string, string][], payload: Uint8Array) {
    return this.client.post("Api/Post-Management/" + shareSecret, payload.buffer, { headers: Object.fromEntries(postHeaders) });
  }

  getPosts(shareId: number, page: number, pageSize: number) {
    return this.client.get<PaginatedResponse<PostSnippet>>("Api/Shares/" + shareId + "/Post-Snippets",
      { params: { page: page, pageSize: pageSize }, headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getPostSnippet(postId: number) {
    return this.client.get<PostSnippet>("Api/Posts/" + postId + "/Snippet", { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  getPostContent(postId: number) {
    return this.client.get<ParsedPostContent>("Api/Posts/" + postId + "/Content", { headers: this.getHeaders() })
      .pipe(shareReplay(1))
      .pipe(
        map(parsedContent => {
          if (parsedContent == null) {
            return null;
          }

          var content = new PostContent();
          content.headers = new Map<string, string[]>(Object.entries(parsedContent.headers));
          content.payload = Uint8Array.from(atob(parsedContent.payload), c => c.charCodeAt(0))
          return content;
        })
      );
  }

  //Likes

  addLike(shareId: number) {
    const userId = this.authService.getUserId();
    return this.client.put("Api/Shares/" + shareId + "/Likes/" + userId, {}, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  removeLike(shareId: number) {
    const userId = this.authService.getUserId();
    return this.client.delete("Api/Shares/" + shareId + "/Likes/" + userId, { headers: this.getHeaders() })
      .pipe(shareReplay(1));
  }

  // Callback

  getCallbackInfo(callbackSecret: string) {
    return this.client.get<CallbackInfo>("Api/Callback/Info/Secret/" + callbackSecret)
      .pipe(shareReplay(1));
  }

  handleVerifyEmailCallback(callbackSecret: string) {
    return this.client.post("Api/Callback-Management/VerifyEmail", { callbackSecret })
      .pipe(shareReplay(1));
  }

  resetPassword(callbackSecret: string, password: string) {
    return this.client.post("Api/Callback-Management/PasswordReset", { callbackSecret, password })
      .pipe(shareReplay(1));
  }
}
