let munny = document.querySelector('.munny-cost')
let parsemunny = parseFloat(munny.innerHTML)

let managerupgradecost = document.querySelector('.manager-upgrade-cost')
let parsemanagerupgradecost = parseFloat(managerupgradecost.innerHTML)
let managerlvl = document.querySelector('.managerlevel')
let managerinc = document.querySelector('.managerincrease')

function incrementMunny() {
    parsemunny += 1
    munny.innerHTML = parsemunny
}

function levelUp() {
    if (parsemunny >= parsemanagerupgradecost) {
        parsemunny -= parsemanagerupgradecost
        munny.innerHTML = parsemunny

        managerlvl.innerHTML ++
    }
}