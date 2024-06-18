var board;
var userMove = false;
var game = new Chess();
var difficulty = 10;
var gameStarted = false;
var divMessage = null;

function ShowEmptyBoard() {
  board = Chessboard('myBoard'); // Empty chess board
}

function HideOverlay() {
  $("#divOverlay").LoadingOverlay("hide");
}

function ShowOverlay(message) {
  $("#divOverlay").LoadingOverlay("show", {
    text: message,
  });
}

function DisableControls() {
  $('#dropdownMenuButton').toggleClass('disabled');
  $('#btnStart').toggleClass('disabled');
  $('#btnStart').prop("disabled", true);
  gameStarted = true;
}

function StartGame() {

  console.log('Start game');
  DisableControls();
  ShowOverlay('Loading...');

  var config = {
    draggable: true,
    dropOffBoard: 'snapback',
    onDrop: onDrop,
    position: 'start',
    //showNotation: false,
  }
  board = Chessboard('myBoard', config);
  const jsonData = {
    difficulty: difficulty.toString()
  };

  fetch('/StartGame', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(jsonData.difficulty)
    })
    .then(res => res.json())
    .then(function (data) {
      HideOverlay();
    })

}

function ItemSelected(difficultyIN) {
  difficulty = difficultyIN;

  if (difficulty == 5)
    divMessage.html('Difficulty : Easy');
  else if (difficulty == 10)
    divMessage.html('Difficulty : Medium');
  else if (difficulty == 20)
    divMessage.html('Difficulty : Hard');
}

function onDrop(source, target, piece, newPos, oldPos, orientation) {

  if (piece.search(/b/) !== -1) {
    return 'snapback'
  }

  var move = game.move({
    from: source,
    to: target,
    promotion: 'q' // NOTE: always promote to a queen for example simplicity
  })

  console.log('user move');
  console.log(move);

  if (move === null)
    return 'snapback';

  var inCheck = game.in_check();
  var inCheckmate = game.in_checkmate();

  if (inCheckmate == true)
    divMessage.html('Stockfush in Checkmate, you win');
  else if (inCheck == true)
    divMessage.html('Stockfush in Check');
  else
    divMessage.html('');

  var oldPosition = Chessboard.objToFen(oldPos);
  var newPosition = Chessboard.objToFen(newPos);

  if (oldPosition == newPosition)
    return false;

  var testPosition = source + target;
  var reversePosition = target + "-" + source;

  const jsonData = {
    fenString: newPosition
  };

  ShowOverlay('Stockfish thinking...');
  fetch('/MakeMoveWithFEN', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(jsonData.fenString)
    })
    .then(res => res.json())
    .then(function (data) {
      if (data != null) {
        console.log('Computer Move: ' + data);
        board.move(data);

        var moveFrom = data.substring(0, 2);
        var moveTo = data.substring(3);

        move = game.move({
          from: moveFrom,
          to: moveTo,
          promotion: 'q' // NOTE: always promote to a queen for example simplicity
        });

        console.log('computer move');
        console.log(move);

        var inCheck = game.in_check();
        var inCheckmate = game.in_checkmate();

        if (inCheckmate == true)
          divMessage.html('You are in Checkmate, you loose');
        else if (inCheck == true)
          divMessage.html('You are in Check');
        else 
          divMessage.html('');

        HideOverlay();
      }
      else {
        console.log('Reverse Move');
        board.move(reversePosition);
      }
    })
}

$(function () {
  divMessage = $('#divMessage');
  $('.dropdown').on("click", (function () {
    if (!gameStarted) $('.dropdown-menu').toggleClass('show');
  }));
  ShowEmptyBoard();
});



