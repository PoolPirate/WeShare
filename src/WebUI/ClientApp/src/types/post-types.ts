export class PostSnippet {
  id: number;
  shareId: number;
  createdAt: Date;
  headersSize: number;
  payloadSize: number;
}

export class PostContent {
  headers: Map<string, string[]>;
  payload: Uint8Array;
}

export interface ParsedPostContent {
  headers: object;
  payload: string;
}

export class PostSendFailure {
  createdAt: Date;
}
export class WebhookPostSendFailure extends PostSendFailure {
  statusCode: number;
  responseLatency: number;
}


export class PostSendInfo {
  postSnippet: PostSnippet;
  postSendFailures: PostSendFailure[];

  received: boolean;
  receivedAt: Date | null;
  attempts: number;
}
