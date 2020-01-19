const uri = 'https://localhost:44307/api';
let todos = [];

function getItems() {
    fetch(uri + '/todoitems')
        .then(response => response.json())
        .then(data => _displayTodoItems(data))
        .catch(error => console.error('Unable to get items.', error));

    fetch(uri + '/tags')
        .then(response => response.json())
        // .then(data => _displayTags(data))
        .then(data => _displayCount('tags-counter', data.length))
        .catch(error => console.error('Unable to get tags.', error));        
}

function addItem() {
    const addNameTextbox = document.getElementById('add-todo-title');

    const item = {
        isComplete: false,
        title: addNameTextbox.value.trim()
    };

    fetch(`${uri}/todoitems`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/todoitems/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function updateItemTitle(id, title) {
    itemId = parseInt(id, 10);
    item = todos.find(item => item.id === itemId);
    item.title = title;

    fetch(`${uri}/todoitems/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));
}

function toggleItemStatus(id) {
    itemId = parseInt(id, 10);
    item = todos.find(item => item.id === itemId);
    item.isComplete = !item.isComplete;

    fetch(`${uri}/todoitems/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));
}

function _displayCount(elementId, itemCount) {
    document.getElementById(elementId).innerText = `${itemCount}`;
}

function _displayTodoItems(data) {
    _displayCount('todos-counter', data.length);
    
    const container = document.getElementById('todos-container');
    container.classList.add('card-columns');
    container.innerHTML = '';

    const button = document.createElement('button');
    button.classList.add('btn', 'btn-sm', 'btn-secondary');

    data.forEach(item => {
        let card = document.createElement('div');
        card.classList.add('card');

        let tags = (item.tags.length > 0) ? '<img src="img/tag.svg"> ' + item.tags.map(t => t.name).join(',') : 'no tags';

        if (item.isComplete) {
            card.innerHTML = `
            <div class="card-body bg-light">
                <h5 class="card-title">${item.title}</h5>
                <h6 class="card-subtitle mb-2 text-muted">#${item.id}, ${tags}</h6>
                <p class="card-text"></p>
                <a href="javascript:void(0)" onclick="toggleItemStatus(${item.id})" class="btn btn-outline-warning">Re-open</a>
                <a href="javascript:void(0)" onclick="deleteItem(${item.id})" class="btn btn-outline-danger">Delete</a>
            </div>
            <div class="card-footer text-muted">
                Last modified: ${item.lastModified} 
            </div>
            `;
        } else {
            card.innerHTML = `
            <div class="card-body">
                <h5 class="card-title">${item.title}</h5>
                <h6 class="card-subtitle mb-2 text-muted">#${item.id}, ${tags}</h6>
                <p class="card-text"></p>
                <a href="javascript:void(0)" onclick="toggleItemStatus(${item.id})" class="btn btn-outline-success">Close</a>
                <a href="javascript:void(0)" onclick="deleteItem(${item.id})" class="btn btn-outline-danger">Delete</a>
            </div>
            <div class="card-footer text-muted">
                Last modified: ${item.lastModified} 
            </div>
            `;
        }

        container.appendChild(card);
        linebreak = document.createElement('br');
        container.appendChild(linebreak);
    });

    todos = data;
}
