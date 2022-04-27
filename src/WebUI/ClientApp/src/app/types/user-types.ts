export class UserSnippet {
  id: number;
  username: string;

  createdAt: Date;
  nickname: string;
}

export class UserLogin {
  userSnippet: UserSnippet;
  token: string;
  expiresIn: number;
}
